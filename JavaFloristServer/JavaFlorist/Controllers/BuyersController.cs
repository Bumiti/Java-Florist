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
        [HttpGet("Product")]
        public async Task<IActionResult> GetBouquets(string? search, double? from, double? to, string? sortBy, int page = 1)
        {
            var bouquets = await _buyerService.GetAllAsync(search, from, to, sortBy, page);

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
            Bouquet bouquet = await _buyerService.GetByIdAsync(id, false);

            if (bouquet == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Bouquet found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, bouquet);
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

        [HttpPost("Cart")]
        public async Task<IActionResult> AddToCart(Bouquet id)
        {
            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var cartItem = await _buyerService.AddToCart(id);

            return StatusCode(StatusCodes.Status200OK, cartItem);
        }

        [HttpPut("Cart/{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            // Tìm sản phẩm trong giỏ hàng dựa vào ID và loại bỏ khỏi giỏ hàng
            var cartItem = await _buyerService.RemoveFromCart(id);

            if (cartItem == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Bouquet found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, cartItem);
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

        [HttpPost("Order"), Authorize]
        public async Task<IActionResult> CreateOrder(CheckOut model)
        {
            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var all = await _buyerService.CreateOrder(model);

            return StatusCode(StatusCodes.Status200OK, all);
        }
    }
}
