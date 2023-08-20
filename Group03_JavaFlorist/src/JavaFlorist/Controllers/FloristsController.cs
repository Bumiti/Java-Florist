using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class FloristsController : ControllerBase
    {
        private readonly IAllService<Florist> _floristService;

        public FloristsController(IAllService<Florist> floristService)
        {
            _floristService = floristService;
        }

        // GET: api/Florists
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFlorists()
        {
            var florists = await _floristService.GetAllAsync();

            if (florists == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Florists in database");
            }

            return StatusCode(StatusCodes.Status200OK, florists);
        }

        // GET: api/Florists/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFlorist(int id, bool includes = true)
        {
            Florist florist = await _floristService.GetByIdAsync(id, includes);

            if (florist == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Florist found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, florist);
        }

        // PUT: api/Florists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutFlorist(int id, Florist florist)
        {
            if (id != florist.Id)
            {
                return BadRequest();
            }

            Florist dbFlorist = await _floristService.UpdateAsync(florist);

            if (dbFlorist == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{florist.Name} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Florists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostFlorist(Florist florist)
        {
            var dbFlorist = await _floristService.AddAsync(florist);

            if (dbFlorist == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{florist} could not be added.");
            }

            return CreatedAtAction("GetFlorist", new { id = florist.Id }, florist);
        }

        // DELETE: api/Florists/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFlorist(int id)
        {
            var florist = await _floristService.GetByIdAsync(id, false);
            (bool status, string message) = await _floristService.DeleteAsync(florist);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, florist);
        }
    }
}
