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
        [HttpGet("Bouquets"), Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> GetBouquets(int page)
        {
            var bouquets = await _floristAllService.GetBouquetAsync(page);

            if (bouquets == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, bouquets);
        }

        [HttpGet("CountBouquet"), Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> CountBouquet()
        {
            var totalBouquet = await _floristAllService.CountBouquetAsync();

            if (totalBouquet == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, totalBouquet);
        }

        [HttpGet("CountBlog"), Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> CountBlog()
        {
            var totalBlog = await _floristAllService.CountBlogAsync();

            if (totalBlog == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, totalBlog);
        }

        // GET: api/Bouquets/5
        [HttpGet("Bouquets/{id}"), Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> GetBouquet(int id, bool includes = true)
        {
            Bouquet bouquet = await _floristAllService.GetBouquetByIdAsync(id, includes);

            if (bouquet == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Bouquet found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, bouquet);
        }

        // PUT: api/Bouquets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Bouquets/{id}"), Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> PutBouquet(int id, Bouquet bouquet)
        {
            if (id != bouquet.Id)
            {
                return BadRequest();
            }

            Bouquet dbBouquet = await _floristAllService.UpdateBouquetAsync(bouquet);

            if (dbBouquet == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{bouquet.Name} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Bouquets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Bouquets"), Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> PostBouquet(Bouquet bouquet)
        {
            var dbBouquet = await _floristAllService.AddBouquetAsync(bouquet);

            if (dbBouquet == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{bouquet.Name} could not be added.");
            }

            return CreatedAtAction("GetBouquet", new { id = bouquet.Id }, bouquet);
        }

        // DELETE: api/Bouquets/5
        [HttpDelete("Bouquets/{id}"), Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> DeleteBouquet(int id)
        {
            var bouquet = await _floristAllService.GetBouquetByIdAsync(id, false);
            (bool status, string message) = await _floristAllService.DeleteBouquetAsync(bouquet);

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
        [Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> GetBlogs(int page)
        {
            var blogs = await _floristAllService.GetBlogAsync(page);

            if (blogs == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Blogs in database");
            }

            return StatusCode(StatusCodes.Status200OK, blogs);
        }

        // GET: api/Blogs/5
        [HttpGet("Blog/{id}")]
        [Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> GetBlog(int id, bool includes = true)
        {
            Blog blog = await _floristAllService.GetBlogByIdAsync(id, includes);

            if (blog == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Blog found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, blog);
        }

        // PUT: api/Blogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Blog/{id}")]
        [Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> PutBlog(int id, Blog blog)
        {
            if (id != blog.Id)
            {
                return BadRequest();
            }

            Blog dbBlog = await _floristAllService.UpdateBlogAsync(blog);

            if (dbBlog == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{blog.Title} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Blogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Blog")]
        [Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> PostBlog(Blog blog)
        {
            var dbBlog = await _floristAllService.AddBlogAsync(blog);

            if (dbBlog == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{blog.Title} could not be added.");
            }

            return CreatedAtAction("GetBlog", new { id = blog.Id }, blog);
        }

        // DELETE: api/Blogs/5
        [HttpDelete("Blog/{id}")]
        [Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _floristAllService.GetBlogByIdAsync(id, false);
            (bool status, string message) = await _floristAllService.DeleteBlogAsync(blog);

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
        [Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _floristAllService.GetOrderAsync();

            if (orders == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Orders in database");
            }

            return StatusCode(StatusCodes.Status200OK, orders);
        }
        [HttpGet("OrderDetail")]
        [Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> GetOrderDetails()
        {
            var orders = await _floristAllService.GetOrderDetailsAsync();

            if (orders == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Orders in database");
            }

            return StatusCode(StatusCodes.Status200OK, orders);
        }
        [HttpGet("OrderDetail/{id}")]
        [Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var orders = await _floristAllService.GetOrderDetailsByOrderIdAsync(id);

            if (orders == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Orders in database");
            }

            return StatusCode(StatusCodes.Status200OK, orders);
        }
        // GET: api/Orders/5
        [HttpGet("Order/{id}")]
        [Authorize(Roles = "Admin,Florist")]
        public async Task<IActionResult> GetOrder(int id, bool includes = true)
        {
            Order order = await _floristAllService.GetOrderByIdAsync(id, includes);

            if (order == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Order found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, order);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754




        [HttpPut("OrderConfirm")]
        [Authorize(Roles = "Admin,Florist")]
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
        [Authorize(Roles = "Admin,Florist")]
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
