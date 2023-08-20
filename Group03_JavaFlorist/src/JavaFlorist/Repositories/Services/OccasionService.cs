using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class OccasionService : IAllService<Occasion>
    {
        private readonly JavaFloristDbContext _context;

        public OccasionService(JavaFloristDbContext context)
        {
            _context = context;
        }
        public async Task<List<Occasion>> GetAllAsync()
        {
            try
            {
                return await _context.Occasions.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Occasion> GetByIdAsync(int id, bool includeProducts = false)
        {
            try
            {
                if (includeProducts) // categories should be included
                {
                    return await _context.Occasions.Include(o => o.Orders)
                        .FirstOrDefaultAsync(i => i.Id == id);
                }
                // Categories should be excluded
                return await _context.Occasions.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Occasion> AddAsync(Occasion occasion)
        {
            try
            {
                await _context.Occasions.AddAsync(occasion);
                await _context.SaveChangesAsync();
                return await _context.Occasions.FindAsync(occasion.Id); // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Occasion> UpdateAsync(Occasion occasion)
        {
            try
            {
                _context.Entry(occasion).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return occasion;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAsync(Occasion occasion)
        {
            try
            {
                var dbOccasion = await _context.Occasions.FindAsync(occasion.Id);

                if (dbOccasion == null)
                {
                    return (false, "Occasion could not be found");
                }

                _context.Occasions.Remove(occasion);
                await _context.SaveChangesAsync();

                return (true, "Occasion got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
