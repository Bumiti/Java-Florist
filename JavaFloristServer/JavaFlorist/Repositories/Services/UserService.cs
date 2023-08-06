using JavaFlorist.Data;
using JavaFlorist.Models.Accounts;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class UserService : IAllService<User>

    {
        private readonly JavaFloristDbContext _context;

        public UserService(JavaFloristDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllAsync()
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Status)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User> GetByIdAsync(int id, bool includes = true)
        {
            try
            {
                if (includes == true) //Users should be included
                {
                    return await _context.Users.Include(u => u.Status)
                        .FirstOrDefaultAsync(i => i.Id == id);
                }
                // Users should be excluded
                return await _context.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User> AddAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user; // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<User> UpdateAsync(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAsync(User user)
        {
            try
            {
                var dbUser = await _context.Categories.FindAsync(user.Id);

                if (dbUser == null)
                {
                    return (false, "User could not be found");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return (true, "User got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
