using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class ReceiverService : IAllService<Receiver>
    {
        private readonly JavaFloristDbContext _context;

        public ReceiverService(JavaFloristDbContext context)
        {
            _context = context;
        }

        public async Task<List<Receiver>> GetAllAsync()
        {
            try
            {
                return await _context.Receivers.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Receiver> GetByIdAsync(int id, bool includes = false)
        {
            try
            {
                return await _context.Receivers.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Receiver> AddAsync(Receiver receiver)
        {
            try
            {
                await _context.Receivers.AddAsync(receiver);
                await _context.SaveChangesAsync();
                return await _context.Receivers.FindAsync(receiver.Id); // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Receiver> UpdateAsync(Receiver receiver)
        {
            try
            {
                _context.Entry(receiver).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return receiver;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAsync(Receiver receiver)
        {
            try
            {
                var dbReceiver = await _context.Receivers.FindAsync(receiver.Id);

                if (dbReceiver == null)
                {
                    return (false, "Receiver could not be found.");
                }

                _context.Receivers.Remove(receiver);
                await _context.SaveChangesAsync();

                return (true, "Receiver got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
