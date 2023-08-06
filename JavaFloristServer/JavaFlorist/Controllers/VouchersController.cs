using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class VouchersController : ControllerBase
    {
        private readonly IAllService<Voucher> _voucherService;

        public VouchersController(IAllService<Voucher> voucherService)
        {
            _voucherService = voucherService;
        }

        // GET: api/Vouchers
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetVouchers()
        {
            var vouchers = await _voucherService.GetAllAsync();

            if (vouchers == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Vouchers in database");
            }

            return StatusCode(StatusCodes.Status200OK, vouchers);
        }

        // GET: api/Vouchers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucher(int id)
        {
            Voucher voucher = await _voucherService.GetByIdAsync(id);

            if (voucher == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Voucher found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, voucher);
        }

        // PUT: api/Vouchers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoucher(int id, Voucher voucher)
        {
            if (id != voucher.Id)
            {
                return BadRequest();
            }

            Voucher dbVoucher = await _voucherService.UpdateAsync(voucher);

            if (dbVoucher == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{voucher.Code} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Vouchers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostVoucher(Voucher voucher)
        {
            var dbVoucher = await _voucherService.AddAsync(voucher);

            if (dbVoucher == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{voucher.Code} could not be added.");
            }

            return CreatedAtAction("GetVoucher", new { id = voucher.Id }, voucher);
        }

        // DELETE: api/Vouchers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            var voucher = await _voucherService.GetByIdAsync(id);
            (bool status, string message) = await _voucherService.DeleteAsync(voucher);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, voucher);
        }
    }
}
