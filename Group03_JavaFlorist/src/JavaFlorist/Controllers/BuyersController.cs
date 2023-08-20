using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyersController : ControllerBase
    {
        private readonly IBuyerService _buyerService;

        public BuyersController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        // GET: api/Bouquets
        [HttpGet("Index")]
        public async Task<IActionResult> GetIndexBouquets(string? search, double? from, double? to, string? sortBy, int page_size, int page = 1)
        {
            var bouquets = await _buyerService.GetAllAsync(search, from, to, sortBy, 8, page);

            if (bouquets == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, bouquets);
        }

        [HttpGet("Product")]
        public async Task<IActionResult> GetShopBouquets(string? search, double? from, double? to, string? sortBy, string? occasion, int page = 1)
        {
            var bouquets = await _buyerService.GetBouquetAsync(search, from, to, sortBy, occasion, page);

            if (bouquets == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, bouquets);
        }

        [HttpGet("Blog")]
        public async Task<IActionResult> GetBlog(int page = 1)
        {
            var bouquets = await _buyerService.GetBlogAsync(page);

            if (bouquets == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, bouquets);
        }

        // GET: api/Bouquets/5
        [HttpGet("Product/{id}")]
        public async Task<IActionResult> GetBouquet(int id)
        {
            Bouquet bouquet = await _buyerService.GetByIdAsync(id, true);

            if (bouquet == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Bouquet found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, bouquet);
        }

        [HttpGet("Blog/{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            Blog blog = await _buyerService.GetBlogByIdAsync(id, true);

            if (blog == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Bouquet found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, blog);
        }

        [HttpGet("Mess/{occastion}")]
        public async Task<IActionResult> GetMessByOccasion(int occastion)
        {
            var occ = await _buyerService.GetMessByOccasionAsync(occastion);

            if (occ == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Message found for occasion: {occastion}");
            }

            return StatusCode(StatusCodes.Status200OK, occ);
        }


        [HttpGet("Cart")]
        public async Task<IActionResult> GetCartItems()
        {
            var cartItems = await _buyerService.GetCartItems();

            if (cartItems == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }
            return StatusCode(StatusCodes.Status200OK, cartItems);
        }

        [HttpGet("CountCart")]
        public async Task<IActionResult> CountCartItems()
        {
            var cartItems = _buyerService.CountCart();

            if (cartItems == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, cartItems);
        }

        [HttpGet("CountBouquet")]
        public async Task<IActionResult> CountBouquet()
        {
            var totalBouquet = _buyerService.CountBouquet();

            if (totalBouquet == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, totalBouquet);
        }

        [HttpGet("CountBlog")]
        public async Task<IActionResult> CountBlog()
        {
            var totalBlog = _buyerService.CountBlog();

            if (totalBlog == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, totalBlog);
        }

        [HttpGet("Cart/{id}")]
        public async Task<IActionResult> GetCartItemsById(int id)
        {
            var cartItems = await _buyerService.GetCartItemsById(id);

            if (cartItems == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, cartItems);
        }

        [HttpPost("Cart")]
        public async Task<IActionResult> AddToCart(Bouquet id)
        {
            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var cartItem = await _buyerService.AddToCart(id);

            return StatusCode(StatusCodes.Status200OK, cartItem);
        }

        [HttpPut("Cart")]
        public async Task<IActionResult> UpdateCart(Cart cart)
        {
            var cartItem = await _buyerService.UpdateCart(cart);

            return StatusCode(StatusCodes.Status200OK, cartItem);
        }

        [HttpGet("ClearCart")]
        public async Task<IActionResult> ClearCart()
        {
            // Tìm sản phẩm trong giỏ hàng dựa vào ID và loại bỏ khỏi giỏ hàng
            _buyerService.ClearCart();

            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpDelete("Cart/{id}")]
        public async Task<IActionResult> DeleteFormCart(int id)
        {
            // Tìm sản phẩm trong giỏ hàng dựa vào ID và loại bỏ khỏi giỏ hàng
            var cartItem = await _buyerService.DeleteFormCart(id);

            if (cartItem == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Bouquet found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, cartItem);
        }

        [HttpPost("Order"), Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOrder(CheckOut model)
        {
            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var all = await _buyerService.CreateOrder(model);

            return StatusCode(StatusCodes.Status200OK, all);
        }

        [HttpGet("Order"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetOrder()
        {
            var order = await _buyerService.GetOrder();

            if (order == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, order);
        }

        [HttpGet("OrderDetails/{id}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var orderDetail = await _buyerService.GetOrderDetails(id);

            if (orderDetail == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, orderDetail);
        }
    }
}
