using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class OrderService : IAllService<Order>
    {
        private readonly JavaFloristDbContext _context;

        public OrderService(JavaFloristDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetAllAsync()
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Receiver).Include(o => o.Status)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Order> GetByIdAsync(int id, bool includes = true)
        {
            try
            {
                if (includes == true) //Orders should be included
                {
                    return await _context.Orders.Include(o => o.Receiver).Include(o => o.Status)
                        .FirstOrDefaultAsync(i => i.Id == id);
                }
                // Orders should be excluded
                return await _context.Orders.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Order> AddAsync(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order; // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            try
            {
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAsync(Order order)
        {
            try
            {
                var dbOrder = await _context.Categories.FindAsync(order.Id);

                if (dbOrder == null)
                {
                    return (false, "Order could not be found");
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                return (true, "Order got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
