using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Models.Emails;
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
        private readonly IMailService _mail;

        public static int PAGE_SIZE { get; set; } = 8;
        public BuyerService(JavaFloristDbContext context, IAccountService accountService, IHttpContextAccessor httpContextAccessor, IMailService mail)
        {
            _context = context;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            _mail = mail;
        }

        public async Task<List<Bouquet>> GetAllAsync(string? search, double? from, double? to, string? sortBy, int page_size, int page = 1)
        {
            try
            {

                var allProducts = _context.Bouquets
                     .Include(b => b.Category).Include(b => b.Florist).Where(b => b.Tag == "Summer").AsQueryable();

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


                var result = PaginatedList<Bouquet>.Create(allProducts, page, page_size);
                return result.ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Bouquet>> GetBouquetAsync(string? search, double? from, double? to, string? sortBy, string? occasionId, int page = 1)
        {
            try
            {
                /*var allProducts = _context.Bouquets
                     .Include(b => b.Category).Include(b => b.Florist).AsQueryable();*/

                var allProducts = _context.Bouquets
                   .Where(b => b.Available == true)
                   .Include(b => b.Category).Include(b => b.Florist)
                   .AsQueryable();

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
                if (occasionId != null)
                {
                    allProducts = allProducts.Where(hh => hh.Category.Name.Equals(occasionId));
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

                //var result = PaginatedList<Bouquet>.Create(allProducts, page, page_size);

                return await allProducts.Skip((page - 1) * 9).Take(9).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Blog>> GetBlogAsync(int page = 1)
        {
            try
            {

                var allProducts = _context.Blogs
                     .Include(b => b.Status).Include(b => b.User)
                     .AsQueryable();


                return allProducts.Skip((page - 1) * 9).Take(9).ToList();

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
        public async Task<Cart> GetCartItemsById(int id)
        {
            return cartItems.FirstOrDefault(item => item.Id == id);
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
                Image = id.Image,
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

        public async void ClearCart()
        {
            cartItems.Clear();
        }

        public async Task<Cart> UpdateCart(Cart cart)
        {
            try
            {
                var existingCartItem = cartItems.FirstOrDefault(c => c.Id == cart.Id);

                if (existingCartItem != null)
                {
                    // Update the properties of the existing cart item
                    existingCartItem.Name = cart.Name;
                    existingCartItem.Price = cart.Price;
                    existingCartItem.Quantity = cart.Quantity;
                    existingCartItem.FloristId = cart.FloristId;
                    return existingCartItem;
                }
                else
                {
                    // Cart item not found, handle accordingly
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return null;
            }

        }

        public async Task<CheckOut> CreateOrder(CheckOut model)
        {
            //Login Already
            //var username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            //var username = _accountService.GetMyName();
            model.CartItems = null;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(model.SenderEmail));
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
                coupon.StatusId = 2;
                _context.Vouchers.Update(coupon);
                await _context.SaveChangesAsync();
            }
            else
            {
                percent = 0;
            }
            //OrderDate
            var receiver = new Receiver
            {
                Name = model.ReciverName,
                Address = model.ReciverAddress,
                Phone = model.ReciverPhone,
                ReceiverDate = model.ReceiverDate,
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
                OrderDate = model.OrderDate,
                ReceiveDate = model.ReceiverDate,
                ReceiverId = receiver.Id,
                Address = receiver.Address,
                // FloristId la BouquetId
                OccasionId = model.Messages, //Occasion
                Amount = total,
                StatusId = 2
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
                if (bouquet.Quantity < 0)
                {
                    bouquet.Available = false;
                }
                _context.Bouquets.Update(bouquet);
                await _context.SaveChangesAsync();
            }

            cartItems.Clear();
            await _context.SaveChangesAsync();
            model.Status = "Completed Order";

            var mailData = new VoucherMail
            {
                To = user.Email,
                From = "javafloristshop@gmail.com",
                DisplayName = "Java Florist",
                Subject = "Order Confirmation",
                Body = @$"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 0;
                    background-color: #f5f5f5;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border-radius: 5px;
                    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                }}
                h1 {{
                    color: #333333;
                }}
                h3 {{
                    color: #333333;
                }}
                p {{
                    color: #555555;
                    line-height: 1.6;
                }}
                strong {{
                    color: #000000;
                }}
                a {{
                    color: #007bff;
                    text-decoration: none;
                }}
                ul {{
                    list-style: none;
                    padding: 0;
                }}
                ul li {{
                    margin-bottom: 10px;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Order Confirmation</h1>
                <p>Thank you for your order at Java Florist! Your order details are as follows:</p>
                <h3>Order Information:</h3>
                <p><strong>Order Number:</strong> {order.Id}</p>
                <p><strong>Order Date:</strong> {order.OrderDate}</p>
                
                <h3>Sender Information:</h3>
                <p><strong>Name:</strong> {model.SenderFullName}</p>
                <p><strong>Mail:</strong> {model.SenderEmail}</p>
                <p><strong>Phone:</strong> {model.SenderPhone}</p>
                <p><strong>Address:</strong> {model.SenderAddress}</p>
                
                <h3>Receiver Information:</h3>
                <p><strong>Name:</strong> {order.Receiver.Name}</p>
                <p><strong>Phone:</strong> {order.Receiver.Phone}</p>
                <p><strong>Address:</strong> {order.Receiver.Address}</p>
                <p><strong>Delivery Date:</strong> {order.Receiver.ReceiverDate}</p>

                <h3>Order Details:</h3>
                <ul>
                {string.Join("", order.OrderDetails.Select(item => $@"
                    <li>
                        <p><strong>Product:</strong> {item.Bouquet.Name}</p>
                        <p><strong>Quantity:</strong> {item.Quantity}</p>
                        <p><strong>Unit Price:</strong> {item.UnitPrice}</p>
                    </li>"))}
                </ul>

                <p><strong>Total Amount:</strong> {order.Amount}</p>

                <p>Your order is being processed and will be delivered soon. Thank you for choosing Java Florist!</p>
                <p>For any inquiries, please <a href='http://localhost:5021'>contact us</a>.</p>
            </div>
        </body>
        </html>"
            };

            await _mail.SendMailAsync(mailData, default);

            return model;
        }

        public async Task<List<Order>> GetOrder()
        {
            try
            {
                var mail = _accountService.GetMyName();
                var userId = _context.Users.FirstOrDefault(u => u.Email.Equals(mail));
                return await _context.Orders
                    .Where(o => o.UserId == userId.Id)
                    .Include(o => o.Receiver).Include(o => o.Status)
                    .Include(o => o.Occasion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<OrderDetail>> GetOrderDetails(int id)
        {
            try
            {
                return await _context.OrderDetails
                    .Where(i => i.OrderId == id)
                    .Include(b => b.Bouquet)
                    .Include(b => b.Order)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int CountCart()
        {
            return cartItems.Count();
        }

        public async Task<List<Occasion>> GetMessByOccasionAsync(int occasion)
        {
            try
            {
                switch (occasion)
                {
                    case 1:
                        return await _context.Occasions
                            .Where(o => o.Type == "Birthday")
                            .ToListAsync();
                    case 2:
                        return await _context.Occasions
                            .Where(o => o.Type == "Get Well")
                            .ToListAsync();
                    // Add more cases for values 3 to 9
                    case 3:
                        return await _context.Occasions
                            .Where(o => o.Type == "Graduation")
                            .ToListAsync();
                    case 4:
                        return await _context.Occasions
                            .Where(o => o.Type == "Grand Opening")
                            .ToListAsync();
                    case 5:
                        return await _context.Occasions
                            .Where(o => o.Type == "Love")
                            .ToListAsync();
                    case 6:
                        return await _context.Occasions
                            .Where(o => o.Type == "Noel")
                            .ToListAsync();
                    case 7:
                        return await _context.Occasions
                            .Where(o => o.Type == "Sympathy - Funerals")
                            .ToListAsync();
                    case 8:
                        return await _context.Occasions
                            .Where(o => o.Type == "Thanks")
                            .ToListAsync();
                    case 9:
                        return await _context.Occasions
                            .Where(o => o.Type == "Wedding")
                            .ToListAsync();
                    default:
                        // Handle the default case if needed
                        throw new ArgumentOutOfRangeException(nameof(occasion), "Invalid occasion value");
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int CountBouquet()
        {
            return (int)Math.Ceiling(_context.Bouquets.Count() / (double)9);
        }

        public int CountBlog()
        {
            return (int)Math.Ceiling(_context.Blogs.Count() / (double)9);

        }

        public async Task<Blog> GetBlogByIdAsync(int id, bool includes = false)
        {
            try
            {
                if (includes == true) //bouquets should be included
                {
                    return await _context.Blogs.Include(b => b.Status).Include(b => b.User)
                        .FirstOrDefaultAsync(i => i.Id == id);
                }
                // Bouquets should be excluded
                return await _context.Blogs.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}