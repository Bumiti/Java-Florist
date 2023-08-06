using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Florist")]
    public class FloristAllsController : ControllerBase
    {
        private readonly IFloristAllService _floristAllService;

        public FloristAllsController(IFloristAllService floristAllService)
        {
            _floristAllService = floristAllService;
        }
        #region Bouquets
        // GET: api/Bouquets
        [HttpGet("Bouquets")]
        public async Task<IActionResult> GetBouquets(int floristId)
        {
            var bouquets = await _floristAllService.GetBouquetAsync(floristId);

            if (bouquets == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, bouquets);
        }

        // GET: api/Bouquets/5
        [HttpGet("Bouquets{id}")]
        public async Task<IActionResult> GetBouquet(int id, int floristId, bool includes = true)
        {
            Bouquet bouquet = await _floristAllService.GetBouquetByIdAsync(id, floristId, includes);

            if (bouquet == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Bouquet found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, bouquet);
        }

        // PUT: api/Bouquets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Bouquets{id}")]
        public async Task<IActionResult> PutBouquet(int id, int floristId, Bouquet bouquet)
        {
            if (id != bouquet.Id)
            {
                return BadRequest();
            }

            Bouquet dbBouquet = await _floristAllService.UpdateBouquetAsync(bouquet, floristId);

            if (dbBouquet == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{bouquet.Name} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Bouquets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Bouquets")]
        public async Task<IActionResult> PostBouquet(Bouquet bouquet, int floristId)
        {
            var dbBouquet = await _floristAllService.AddBouquetAsync(bouquet, floristId);

            if (dbBouquet == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{bouquet.Name} could not be added.");
            }

            return CreatedAtAction("GetBouquet", new { id = bouquet.Id }, bouquet);
        }

        // DELETE: api/Bouquets/5
        [HttpDelete("Bouquets{id}")]
        public async Task<IActionResult> DeleteBouquet(int id, int floristId)
        {
            var bouquet = await _floristAllService.GetBouquetByIdAsync(id, floristId, false);
            (bool status, string message) = await _floristAllService.DeleteBouquetAsync(bouquet, floristId);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, bouquet);
        }
        #endregion

        #region Blogs
        // GET: api/Blogs
        [HttpGet("Blog")]
        public async Task<IActionResult> GetBlogs(int userId)
        {
            var blogs = await _floristAllService.GetBlogAsync(userId);

            if (blogs == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Blogs in database");
            }

            return StatusCode(StatusCodes.Status200OK, blogs);
        }

        // GET: api/Blogs/5
        [HttpGet("Blog{id}")]
        public async Task<IActionResult> GetBlog(int id, int userId)
        {
            Blog blog = await _floristAllService.GetBlogByIdAsync(id, userId, true);

            if (blog == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Blog found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, blog);
        }

        // PUT: api/Blogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Blog{id}")]
        public async Task<IActionResult> PutBlog(int id, int userId, Blog blog)
        {
            if (id != blog.Id)
            {
                return BadRequest();
            }

            Blog dbBlog = await _floristAllService.UpdateBlogAsync(blog, userId);

            if (dbBlog == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{blog.Title} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Blogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Blog")]
        public async Task<IActionResult> PostBlog(Blog blog, int userId)
        {
            var dbBlog = await _floristAllService.AddBlogAsync(blog, userId);

            if (dbBlog == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{blog.Title} could not be added.");
            }

            return CreatedAtAction("GetBlog", new { id = blog.Id }, blog);
        }

        // DELETE: api/Blogs/5
        [HttpDelete("Blog{id}")]
        public async Task<IActionResult> DeleteBlog(int id, int userId)
        {
            var blog = await _floristAllService.GetBlogByIdAsync(id, userId, false);
            (bool status, string message) = await _floristAllService.DeleteBlogAsync(blog, userId);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, blog);
        }
        #endregion

        #region Orders

        // GET: api/Orders
        [HttpGet("Order")]
        public async Task<IActionResult> GetOrders(int floristId)
        {
            var orders = await _floristAllService.GetOrderAsync(floristId);

            if (orders == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Orders in database");
            }

            return StatusCode(StatusCodes.Status200OK, orders);
        }

        // GET: api/Orders/5
        [HttpGet("Order{id}")]
        public async Task<IActionResult> GetOrder(int id, int floristId, bool includes = true)
        {
            Order order = await _floristAllService.GetOrderByIdAsync(id, floristId, includes);

            if (order == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Order found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, order);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Order{id}")]
        public async Task<IActionResult> PutOrder(int id, int floristId, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            Order dbOrder = await _floristAllService.UpdateOrderAsync(order, floristId);

            if (dbOrder == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{order.Id} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Order")]
        public async Task<IActionResult> PostOrder(Order order, int floristId)
        {
            var dbOrder = await _floristAllService.AddOrderAsync(order, floristId);

            if (dbOrder == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{order.Id} could not be added.");
            }

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("Order{id}")]
        public async Task<IActionResult> DeleteOrder(int id, int floristId)
        {
            var order = await _floristAllService.GetOrderByIdAsync(id, floristId, false);
            (bool status, string message) = await _floristAllService.DeleteOrderAsync(order, floristId);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, order);
        }

        [HttpPut("OrderConfirm")]
        public async Task<IActionResult> VerifyOrderSuccess(Order order)
        {
            if (order.Id == null)
            {
                return BadRequest();
            }

            Order dbOrder = await _floristAllService.UpdateOrderStatusDeliAsync(order);

            if (dbOrder == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{order.Id} could not be updated");
            }
            else
            {
                return Ok();

            }
        }

        [HttpPut("OrderCancel")]
        public async Task<IActionResult> VerifyOrderCancel(Order order)
        {
            if (order.Id == null)
            {
                return BadRequest();
            }

            Order dbOrder = await _floristAllService.UpdateOrderStatusCancelAsync(order);

            if (dbOrder == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{order.Id} could not be updated");
            }
            else
            {
                return Ok();

            }
        }
        #endregion
    }
}
