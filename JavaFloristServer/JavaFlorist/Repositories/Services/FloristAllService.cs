using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class FloristAllService : IFloristAllService
    {
        private readonly JavaFloristDbContext _context;

        public FloristAllService(JavaFloristDbContext context)
        {
            _context = context;
        }

        #region Bouquet
        public async Task<List<Bouquet>> GetBouquetAsync(int floristId)
        {
            try
            {
                return await _context.Bouquets
                    .Include(b => b.Category).Include(b => b.Florist)
                    .Where(b => b.FloristId.Equals(floristId))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Bouquet> GetBouquetByIdAsync(int id, int floristId, bool includes = false)
        {
            try
            {
                if (includes == true) //bouquets should be included
                {
                    return await _context.Bouquets.Include(b => b.Category).Include(b => b.Florist)
                        .Where(b => b.FloristId.Equals(floristId))
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

        public async Task<Bouquet> AddBouquetAsync(Bouquet bouquet, int floristId)
        {
            try
            {
                bouquet.FloristId = floristId;
                await _context.Bouquets.AddAsync(bouquet);
                await _context.SaveChangesAsync();
                return bouquet; // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Bouquet> UpdateBouquetAsync(Bouquet bouquet, int floristId)
        {
            try
            {
                bouquet.FloristId = floristId;
                _context.Entry(bouquet).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return bouquet;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteBouquetAsync(Bouquet bouquet, int floristId)
        {
            try
            {
                var dbBouquet = await _context.Bouquets.FindAsync(bouquet.Id);

                if (dbBouquet == null)
                {
                    return (false, "Bouquet could not be found");
                }

                _context.Bouquets.Remove(bouquet);
                await _context.SaveChangesAsync();

                return (true, "Bouquet got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
        #endregion

        #region Blog
        public async Task<List<Blog>> GetBlogAsync(int userId)
        {
            try
            {
                return await _context.Blogs
                    .Include(b => b.Status).Include(b => b.User)
                    .Where(b => b.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Blog> GetBlogByIdAsync(int id, int userId, bool includes = false)
        {
            try
            {
                if (includes == true) // blogs should be included
                {
                    return await _context.Blogs.Include(b => b.Status).Include(b => b.User)
                        .Where(b => b.UserId == userId)
                        .FirstOrDefaultAsync(i => i.Id == id);
                }
                // Blogs should be excluded
                return await _context.Blogs.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Blog> AddBlogAsync(Blog blog, int userId)
        {
            try
            {

                blog.UserId = userId;
                blog.StatusId = 1;
                await _context.Blogs.AddAsync(blog);
                await _context.SaveChangesAsync();

                return blog; // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Blog> UpdateBlogAsync(Blog blog, int userId)
        {
            try
            {
                blog.UserId = userId;
                _context.Entry(blog).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return blog;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteBlogAsync(Blog blog, int userId)
        {
            try
            {
                var dbBlog = await _context.Blogs.FindAsync(blog.Id);

                if (dbBlog == null)
                {
                    return (false, "Blog could not be found");
                }

                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();

                return (true, "Blog got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
        #endregion

        #region Oder
        public async Task<List<Order>> GetOrderAsync(int floristId)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.Receiver).Include(o => o.Status)
                    .Where(o => o.FloristId.Equals(floristId))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Order> GetOrderByIdAsync(int id, int floristId, bool includes = false)
        {
            try
            {
                if (includes == true) //Orders should be included
                {
                    return await _context.Orders.Include(o => o.Receiver).Include(o => o.Status)
                        .Where(o => o.FloristId.Equals(floristId))
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

        public async Task<Order> AddOrderAsync(Order order, int floristId)
        {
            try
            {
                order.FloristId = floristId;
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order; // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Order> UpdateOrderAsync(Order order, int floristId)
        {
            try
            {
                order.FloristId = floristId;
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteOrderAsync(Order order, int floristId)
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

        public async Task<Order> UpdateOrderStatusDeliAsync(Order order)
        {
            try
            {
                var orderChange = await _context.Orders.FirstOrDefaultAsync(u => u.Id == order.Id);
                // Update the user's StatusId to 2
                orderChange.StatusId = 4;
                _context.Orders.Update(orderChange);
                await _context.SaveChangesAsync();
                return orderChange;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Order> UpdateOrderStatusCancelAsync(Order order)
        {
            try
            {
                var orderChange = await _context.Orders.FirstOrDefaultAsync(u => u.Id == order.Id);
                // Update the user's StatusId to 2
                orderChange.StatusId = 6;
                _context.Orders.Update(orderChange);
                await _context.SaveChangesAsync();
                return orderChange;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
