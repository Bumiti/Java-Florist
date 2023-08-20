using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class CategoryService : IAllService<Category>
    {
        private readonly JavaFloristDbContext _context;

        public CategoryService(JavaFloristDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetAllAsync()
        {
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Category> GetByIdAsync(int id, bool includeProducts = false)
        {
            try
            {
                if (includeProducts) // categories should be included
                {
                    return await _context.Categories.Include(p => p.Bouquets)
                        .FirstOrDefaultAsync(i => i.Id == id);
                }
                // Categories should be excluded
                return await _context.Categories.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Category> AddAsync(Category category)
        {
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return await _context.Categories.FindAsync(category.Id); // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            try
            {
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return category;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAsync(Category category)
        {
            try
            {
                var dbCategory = await _context.Categories.FindAsync(category.Id);

                if (dbCategory == null)
                {
                    return (false, "Category could not be found");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return (true, "Category got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
