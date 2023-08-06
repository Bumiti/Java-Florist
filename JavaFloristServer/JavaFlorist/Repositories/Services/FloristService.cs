using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class FloristService : IAllService<Florist>
    {
        private readonly JavaFloristDbContext _context;

        public FloristService(JavaFloristDbContext context)
        {
            _context = context;
        }
        public async Task<List<Florist>> GetAllAsync()
        {
            try
            {
                return await _context.Florists
                    .Include(b => b.Status).Include(b => b.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Florist> GetByIdAsync(int id, bool includes = true)
        {
            try
            {
                if (includes == true) //florists should be included
                {
                    return await _context.Florists.Include(b => b.Status).Include(b => b.User)
                        .FirstOrDefaultAsync(i => i.Id == id);
                }
                // Florists should be excluded
                return await _context.Florists.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Florist> AddAsync(Florist florist)
        {
            try
            {
                await _context.Florists.AddAsync(florist);
                await _context.SaveChangesAsync();
                return florist; // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Florist> UpdateAsync(Florist florist)
        {
            try
            {
                _context.Entry(florist).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return florist;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAsync(Florist florist)
        {
            try
            {
                var dbFlorist = await _context.Categories.FindAsync(florist.Id);

                if (dbFlorist == null)
                {
                    return (false, "Florist could not be found");
                }

                _context.Florists.Remove(florist);
                await _context.SaveChangesAsync();

                return (true, "Florist got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
