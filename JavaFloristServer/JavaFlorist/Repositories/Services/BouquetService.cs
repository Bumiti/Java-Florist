using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class BouquetService : IAllService<Bouquet>
    {
        private readonly JavaFloristDbContext _context;

        public BouquetService(JavaFloristDbContext context)
        {
            _context = context;
        }
        public async Task<List<Bouquet>> GetAllAsync()
        {
            try
            {
                return await _context.Bouquets
                    .Include(b => b.Category).Include(b => b.Florist)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Bouquet> GetByIdAsync(int id, bool includes = false)
        {
            try
            {
                if (includes == true) //bouquets should be included
                {
                    return await _context.Bouquets.Include(b => b.Category).Include(b => b.Florist)
                        .FirstOrDefaultAsync(i => i.Id == id);
                }
                // Bouquets should be excluded
                return await _context.Bouquets.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Bouquet> AddAsync(Bouquet bouquet)
        {
            try
            {
                await _context.Bouquets.AddAsync(bouquet);
                await _context.SaveChangesAsync();
                return bouquet; // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Bouquet> UpdateAsync(Bouquet bouquet)
        {
            try
            {
                _context.Entry(bouquet).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return bouquet;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAsync(Bouquet bouquet)
        {
            try
            {
                var dbBouquet = await _context.Bouquets.FindAsync(bouquet.Id);

                if (dbBouquet == null)
                {
                    return (false, "Bouquet could not be found");
                }

                _context.Bouquets.Remove(bouquet);
                await _context.SaveChangesAsync();

                return (true, "Bouquet got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
