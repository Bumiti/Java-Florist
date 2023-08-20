using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class VoucherService : IAllService<Voucher>
    {
        private readonly JavaFloristDbContext _context;

        public VoucherService(JavaFloristDbContext context)
        {
            _context = context;
        }
        public async Task<List<Voucher>> GetAllAsync()
        {
            try
            {
                return await _context.Vouchers.Include(b => b.Status).Include(b => b.User).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Voucher> GetByIdAsync(int id, bool includes = false)
        {
            try
            {
                return await _context.Vouchers.Include(b => b.Status).Include(b => b.User).FirstOrDefaultAsync(b=>b.Equals(id));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Voucher> AddAsync(Voucher voucher)
        {
            try
            {
                await _context.Vouchers.AddAsync(voucher);
                await _context.SaveChangesAsync();
                return await _context.Vouchers.FindAsync(voucher.Id); // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Voucher> UpdateAsync(Voucher voucher)
        {
            try
            {
                _context.Entry(voucher).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return voucher;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAsync(Voucher voucher)
        {
            try
            {
                var dbVoucher = await _context.Vouchers.FindAsync(voucher.Id);

                if (dbVoucher == null)
                {
                    return (false, "Voucher could not be found.");
                }

                _context.Vouchers.Remove(voucher);
                await _context.SaveChangesAsync();

                return (true, "Voucher got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
