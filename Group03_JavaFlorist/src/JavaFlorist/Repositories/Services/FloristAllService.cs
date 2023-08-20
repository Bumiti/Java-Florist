using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class FloristAllService : IFloristAllService
    {
        private readonly JavaFloristDbContext _context;
        private readonly IAccountService _accountService;

        public FloristAllService(JavaFloristDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        #region Bouquet
        public async Task<List<Bouquet>> GetBouquetAsync(int page)
        {
            try
            {
                var userMail = _accountService.GetMyName();
                var florist = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userMail);
                return await _context.Bouquets
                    .Where(b => b.FloristId.Equals(florist.Id))
                    .Include(b => b.Category).Include(b => b.Florist)
                    .Skip((page - 1) * 10).Take(10)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Bouquet> GetBouquetByIdAsync(int id, bool includes)
        {
            try
            {
                if (includes == true) //bouquets should be included
                {
                    return await _context.Bouquets.Include(b => b.Category).Include(b => b.Florist)
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

        public async Task<Bouquet> AddBouquetAsync(Bouquet bouquet)
        {
            try
            {
                //bouquet.FloristId = floristId;
                var userMail = _accountService.GetMyName();
                var florist = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userMail);
                bouquet.FloristId = florist.Id;
                await _context.Bouquets.AddAsync(bouquet);
                await _context.SaveChangesAsync();
                return bouquet; // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Bouquet> UpdateBouquetAsync(Bouquet bouquet)
        {
            try
            {
                //bouquet.FloristId = floristId;
                var userMail = _accountService.GetMyName();
                var florist = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userMail);
                bouquet.FloristId = florist.Id;
                _context.Entry(bouquet).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return bouquet;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteBouquetAsync(Bouquet bouquet)
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
        public async Task<List<Blog>> GetBlogAsync(int page)
        {
            try
            {
                var userMail = _accountService.GetMyName();
                var florist = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userMail);

                return await _context.Blogs
                    .Where(b => b.UserId.Equals(florist.UserId))
                     .Include(b => b.Status).Include(b => b.User)
                    .Skip((page - 1) * 10).Take(10)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Blog> GetBlogByIdAsync(int id, bool includes = false)
        {
            try
            {
                if (includes == true) // blogs should be included
                {
                    return await _context.Blogs.Include(b => b.Status).Include(b => b.User)
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

        public async Task<Blog> AddBlogAsync(Blog blog)
        {
            try
            {
                var userMail = _accountService.GetMyName();
                var florist = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userMail);
                blog.UserId = florist.UserId;
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

        public async Task<Blog> UpdateBlogAsync(Blog blog)
        {
            try
            {
                var userMail = _accountService.GetMyName();
                var florist = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userMail);
                blog.UserId = florist.UserId;
                _context.Entry(blog).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return blog;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteBlogAsync(Blog blog)
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
        public async Task<List<Order>> GetOrderAsync()
        {
            try
            {
                var userId = _accountService.GetMyName();
                var florist = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userId);

                var order = await _context.Orders
                    .Include(o => o.Receiver)
                    .Include(o => o.Status)
                    .Include(o => o.Occasion)
                    /*.Include(o=>o.OrderDetails)*/
                    .Where(o => o.OrderDetails.Any(od =>
                        od.Bouquet.Florist.Email == userId))
                    .ToListAsync();

                return order;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<OrderDetail>> GetOrderDetailsAsync()
        {
            try
            {
                var userId = _accountService.GetMyName();
                var florist = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userId);

                var orderDetails = await _context.OrderDetails
                    .Include(od => od.Bouquet)
                        .ThenInclude(b => b.Florist)
                    .Include(od => od.Order)
                    .Where(od => od.Bouquet.Florist.Email == userId)
                    .ToListAsync();

                return orderDetails;

                /*var order = await _context.Orders
                   .Include(o => o.Receiver).Include(o => o.Status)
                   .Where(o => o.Id.Equals(orderDetailId))
                   .ToListAsync();*/
                /*foreach(var item in order)
                {
                    return 
                }*/
            }
            catch (Exception ex)
            {
                return null;
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
        public async Task<Order> GetOrderByIdAsync(int id, bool includes = false)
        {
            var userId = _accountService.GetMyName();
            var floristId = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userId);
            try
            {
                if (includes == true) //Orders should be included
                {
                    return await _context.Orders.Include(o => o.Receiver).Include(o => o.Status)
                        //.Where(o => o.FloristId.Equals(floristId))
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

        public async Task<int> CountBouquetAsync()
        {
            var userMail = _accountService.GetMyName();
            var florist = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userMail);

            var totalItems = await _context.Bouquets
                .Where(b => b.FloristId.Equals(florist.Id))
                .CountAsync();

            return (int)Math.Ceiling(totalItems / (double)10);
        }

        public async Task<int> CountBlogAsync()
        {
            var userMail = _accountService.GetMyName();
            var florist = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userMail);

            var totalItems = await _context.Blogs
                .Where(b => b.UserId.Equals(florist.UserId))
                .CountAsync();

            return (int)Math.Ceiling(totalItems / (double)10);
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            try
            {
                var userId = _accountService.GetMyName();
                var florist = await _context.Florists.FirstOrDefaultAsync(f => f.Email == userId);

                var orderDetails = await _context.OrderDetails
                    .Include(od => od.Bouquet)
                        .ThenInclude(b => b.Florist)
                    .Include(od => od.Order)
                    .Where(od => od.Bouquet.Florist.Email == userId && od.OrderId == orderId)
                    .ToListAsync();

                return orderDetails;

                /*var order = await _context.Orders
                   .Include(o => o.Receiver).Include(o => o.Status)
                   .Where(o => o.Id.Equals(orderDetailId))
                   .ToListAsync();*/
                /*foreach(var item in order)
                {
                    return 
                }*/
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
