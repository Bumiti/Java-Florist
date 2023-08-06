using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IAllService<OrderDetail> _orderDetailService ;

        public OrderDetailsController(IAllService<OrderDetail> orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails()
        {
            var orderDetails = await _orderDetailService.GetAllAsync();

            if (orderDetails == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No OrderDetails in database");
            }

            return StatusCode(StatusCodes.Status200OK, orderDetails);
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetail(int id, bool includes=true)
        {
            OrderDetail orderDetail = await _orderDetailService.GetByIdAsync(id, includes);

            if (orderDetail == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No orderDetail found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, orderDetail);
        }

        // PUT: api/OrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return BadRequest();
            }

            OrderDetail dbOrderDetail = await _orderDetailService.UpdateAsync(orderDetail);

            if (dbOrderDetail == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{orderDetail.Id} could not be updated");
            }

            return NoContent();
        }

        // POST: api/OrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostOrderDetail(OrderDetail orderDetail)
        {
            var dbOrderDetail = await _orderDetailService.AddAsync(orderDetail);

            if (dbOrderDetail == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{orderDetail.Id} could not be added.");
            }

            return CreatedAtAction("GetOrderDetail", new { id = orderDetail.Id }, orderDetail);
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _orderDetailService.GetByIdAsync(id, false);
            (bool status, string message) = await _orderDetailService.DeleteAsync(orderDetail);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, orderDetail);
        }
    }
}
