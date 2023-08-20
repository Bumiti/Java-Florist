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
using JavaFlorist.Repositories.Services;
using Microsoft.AspNetCore.Authorization;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class OccasionsController : ControllerBase
    {
        private readonly IAllService<Occasion> _occasionService;

        public OccasionsController(IAllService<Occasion> occasionService)
        {
            _occasionService = occasionService;
        }

        // GET: api/Occasions
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOccasions()
        {
            var occasions = await _occasionService.GetAllAsync();

            if (occasions == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No occasions in database");
            }

            return StatusCode(StatusCodes.Status200OK, occasions);
        }

        // GET: api/Occasions/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOccasion(int id, bool includeProducts)
        {
            Occasion occasion = await _occasionService.GetByIdAsync(id, includeProducts);

            if (occasion == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Occasion found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, occasion);
        }

        // PUT: api/Occasions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutOccasion(int id, Occasion occasion)
        {
            if (id != occasion.Id)
            {
                return BadRequest();
            }

            Occasion dbOccasion = await _occasionService.UpdateAsync(occasion);

            if (dbOccasion == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{occasion.Message} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Occasions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostOccasion(Occasion occasion)
        {
            var dbOccasion = await _occasionService.AddAsync(occasion);

            if (dbOccasion == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{occasion.Message} could not be added.");
            }

            return CreatedAtAction("GetOccasion", new { id = occasion.Id }, occasion);
        }

        // DELETE: api/Occasions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOccasion(int id)
        {
            var occasion = await _occasionService.GetByIdAsync(id, false);
            (bool status, string message) = await _occasionService.DeleteAsync(occasion);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, occasion);
        }
    }
}
