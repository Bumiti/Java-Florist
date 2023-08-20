using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IAllService<Order> _orderService;
        private readonly JavaFloristDbContext _context;

        public OrdersController(IAllService<Order> orderService, JavaFloristDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllAsync();

            if (orders == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Orders in database");
            }

            return StatusCode(StatusCodes.Status200OK, orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrder(int id, bool includes = true)
        {
            Order order = await _orderService.GetByIdAsync(id, includes);

            if (order == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Order found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, order);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            Order dbOrder = await _orderService.UpdateAsync(order);

            if (dbOrder == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{order.Id} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostOrder(Order order)
        {
            var dbOrder = await _orderService.AddAsync(order);

            if (dbOrder == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{order.Id} could not be added.");
            }

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetByIdAsync(id, false);
            (bool status, string message) = await _orderService.DeleteAsync(order);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, order);
        }
    }
}
