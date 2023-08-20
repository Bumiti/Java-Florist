using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class BouquetService : IBouquetService
    {
        private readonly JavaFloristDbContext _context;

        public BouquetService(JavaFloristDbContext context)
        {
            _context = context;
        }

        public async Task<List<Bouquet>> GetAllAsync(int page = 1)
        {
            try
            {
                var allProducts = _context.Bouquets
                   .Where(b => b.Available == true)
                   .Include(b => b.Category).Include(b => b.Florist).AsQueryable();

                return await allProducts.Skip((page - 1) * 10).Take(10).ToListAsync();
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

                _context.Bouquets.Remove(dbBouquet);
                await _context.SaveChangesAsync();

                return (true, "Bouquet got deleted.");
            }
            catch (Exception ex)
            {
                /*string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return (false, $"An error occurred. Error Message: {errorMessage}");*/
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }

        public int CountBouquet()
        {
            return (int)Math.Ceiling(_context.Bouquets.Count() / (double)10);
        }
    }
}
