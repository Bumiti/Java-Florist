using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class StatusService :IAllService<Status>
    {
        private readonly JavaFloristDbContext _context;

        public StatusService(JavaFloristDbContext context)
        {
            _context = context;
        }
        public async Task<List<Status>> GetAllAsync()
        {
            try
            {
                return await _context.Statuses.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Status> GetByIdAsync(int id, bool includes = false)
        {
            try
            {
                return await _context.Statuses.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Status> AddAsync(Status status)
        {
            try
            {
                await _context.Statuses.AddAsync(status);
                await _context.SaveChangesAsync();
                return await _context.Statuses.FindAsync(status.Id); // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Status> UpdateAsync(Status status)
        {
            try
            {
                _context.Entry(status).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return status;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAsync(Status status)
        {
            try
            {
                var dbStatus = await _context.Statuses.FindAsync(status.Id);

                if (dbStatus == null)
                {
                    return (false, "Status could not be found.");
                }

                _context.Statuses.Remove(status);
                await _context.SaveChangesAsync();

                return (true, "Status got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
