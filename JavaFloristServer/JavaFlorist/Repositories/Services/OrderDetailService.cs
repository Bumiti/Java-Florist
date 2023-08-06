using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class OrderDetailService :IAllService<OrderDetail>
    {
        private readonly JavaFloristDbContext _context;

        public OrderDetailService(JavaFloristDbContext context)
        {
            _context = context;
        }
        public async Task<List<OrderDetail>> GetAllAsync()
        {
            try
            {
                return await _context.OrderDetails
                    .Include(b => b.Bouquet).Include(b => b.Order)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<OrderDetail> GetByIdAsync(int id, bool includes = true)
        {
            try
            {
                if (includes == true) //OrderDetails should be included
                {
                    return await _context.OrderDetails.Include(b => b.Bouquet).Include(b => b.Order)
                        .FirstOrDefaultAsync(i => i.Id == id);
                }
                // OrderDetails should be excluded
                return await _context.OrderDetails.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<OrderDetail> AddAsync(OrderDetail orderDetail)
        {
            try
            {
                await _context.OrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();
                return orderDetail; // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<OrderDetail> UpdateAsync(OrderDetail orderDetail)
        {
            try
            {
                _context.Entry(orderDetail).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return orderDetail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAsync(OrderDetail orderDetail)
        {
            try
            {
                var dbOrderDetail = await _context.Categories.FindAsync(orderDetail.Id);

                if (dbOrderDetail == null)
                {
                    return (false, "OrderDetail could not be found");
                }

                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();

                return (true, "OrderDetail got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
