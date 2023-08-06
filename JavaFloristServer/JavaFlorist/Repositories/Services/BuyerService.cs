using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;
using Receiver = JavaFlorist.Models.Receiver;

namespace JavaFlorist.Repositories.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly JavaFloristDbContext _context;
        private static List<Cart> cartItems = new List<Cart>();
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public static int PAGE_SIZE { get; set; } = 4;
        public BuyerService(JavaFloristDbContext context, IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Bouquet>> GetAllAsync(string? search, double? from, double? to, string? sortBy, int page = 1)
        {
            try
            {
                /*return await _context.Bouquets
                    .Include(b => b.Category).Include(b => b.Florist)
                    .ToListAsync();*/

                var allProducts = _context.Bouquets
                     .Include(b => b.Category).Include(b => b.Florist).AsQueryable();

                #region Filtering
                if (!string.IsNullOrEmpty(search))
                {
                    allProducts = allProducts.Where(hh => hh.Name.Contains(search));
                }
                if (from.HasValue)
                {
                    allProducts = allProducts.Where(hh => hh.PriceAfter >= from);
                }
                if (to.HasValue)
                {
                    allProducts = allProducts.Where(hh => hh.PriceAfter <= to);
                }
                #endregion


                #region Sorting
                //Default sort by Name (TenHh)
                allProducts = allProducts.OrderBy(hh => hh.Name);

                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy)
                    {
                        case "tenhh_desc": allProducts = allProducts.OrderByDescending(hh => hh.Name); break;
                        case "gia_asc": allProducts = allProducts.OrderBy(hh => hh.PriceAfter); break;
                        case "gia_desc": allProducts = allProducts.OrderByDescending(hh => hh.PriceAfter); break;
                    }
                }
                #endregion


                var result = PaginatedList<Bouquet>.Create(allProducts, page, PAGE_SIZE);
                return result.ToList();
                /*return await result.Select(hh => new Bouquet
                {
                    Id = hh.Id,
                    Name = hh.Name,
                    PriceAfter = hh.PriceAfter,
                    CategoryId = hh.Category?.Id
                }).ToList();*/

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Bouquet> GetByIdAsync(int id, bool includes = false)
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

        public async Task<List<Cart>> GetCartItems()
        {
            return cartItems;
        }

        public async Task<List<Cart>> AddToCart(Bouquet id)
        {
            //var bouquet = await _context.Bouquets.FirstOrDefaultAsync(b => b.Id == id.Id);

            //Add to cart
            var newItem = new Cart
            {
                Id = id.Id,
                Name = id.Name,
                Price = id.PriceAfter,
                Quantity = 1,
                FloristId = id.FloristId
            };

            Cart existingItem = cartItems.FirstOrDefault(item => item.Id == id.Id);


            if (existingItem != null)
            {
                // Nếu đã có, tăng số lượng sản phẩm
                existingItem.Quantity += newItem.Quantity;
            }
            else
            {
                // Nếu chưa có, thêm sản phẩm mới vào giỏ hàng
                cartItems.Add(newItem);
            }

            return cartItems;
        }

        public async Task<List<Cart>> DeleteFormCart(int id)
        {
            Cart existingItem = cartItems.FirstOrDefault(item => item.Id == id);
            if (existingItem != null)
            {
                cartItems.Remove(existingItem);
            }

            return cartItems;
        }

        public async Task<List<Cart>> RemoveFromCart(int id)
        {
            Cart existingItem = cartItems.FirstOrDefault(item => item.Id == id);
            if (existingItem != null)
            {
                existingItem.Quantity -= 1;

                if (existingItem.Quantity == 0)
                {
                    cartItems.Remove(existingItem);
                }
            }
            return cartItems;
        }

        public async Task<CheckOut> CreateOrder(CheckOut model)
        {
            //Login Already
            //var username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var username = await _accountService.GetMyName();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(username));
            if (user != null)
            {
                model.SenderFullName = user.FullName;
                model.SenderEmail = user.Email;
                model.SenderPhone = user.Phone;
                model.SenderAddress = user.Address;
            }
            var coupon = await _context.Vouchers.FirstOrDefaultAsync(vouchers => vouchers.UserId == user.Id && vouchers.Code == model.Code);
            double? percent = 0;
            if (coupon != null)
            {
                percent = coupon.DiscountPercent;
            }
            else
            {
                percent = 0;
            }
            //OrderDate
            model.OrderDate = DateTime.Now;
            var receiver = new Receiver
            {
                Name = model.ReciverName,
                Address = model.ReciverAddress,
                Phone = model.ReciverPhone,
                ReceiverDate = model.OrderDate?.AddHours(5),
                Message = model.Messages
            };
            _context.Receivers.Add(receiver);
            await _context.SaveChangesAsync();
            double? amount = 0;
            double? total = 0;
            foreach (var item in cartItems)
            {
                amount += item.Quantity * item.Price;
                total = amount - amount * percent / 100;
            }
            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                ReceiveDate = DateTime.Now.AddHours(5),
                ReceiverId = receiver.Id,
                Address = receiver.Address,
                FloristId = null,// FloristId la BouquetId
                BouquetBrief = model.BouquetBrief, //Occasion
                Messages = model.Messages,
                Amount = total,
                StatusId = 3
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cartItems)
            {
                var orderDetails = new OrderDetail
                {
                    OrderId = order.Id,
                    BouquetId = item.Id,
                    UnitPrice = item.Price,
                    Quantity = item.Quantity,
                    Discount = percent
                };
                _context.OrderDetails.Add(orderDetails);
                await _context.SaveChangesAsync();
            }

            foreach (var item in cartItems)
            {
                var bouquet = await _context.Bouquets.FirstOrDefaultAsync(b => b.Id.Equals(item.Id));
                bouquet.Quantity -= item.Quantity;
                _context.Bouquets.Update(bouquet);
                await _context.SaveChangesAsync();
            }

            cartItems.Clear();
            await _context.SaveChangesAsync();
            model.Status = "Order Pending";

            coupon.Type = "Used";
            _context.Vouchers.Update(coupon);
            await _context.SaveChangesAsync();

            return model;


        }
    }
}