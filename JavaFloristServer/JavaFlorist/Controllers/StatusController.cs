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
    public class StatusController : ControllerBase
    {
        private readonly IAllService<Status> _statusService;

        public StatusController(IAllService<Status> statusService)
        {
            _statusService = statusService;
        }

        // GET: api/Statuses
        [HttpGet]
        public async Task<IActionResult> GetStatuses()
        {
            var status = await _statusService.GetAllAsync();

            if (status == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Statuses in database");
            }

            return StatusCode(StatusCodes.Status200OK, status);
        }

        // GET: api/Statuses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStatus(int id)
        {
            Status status = await _statusService.GetByIdAsync(id);

            if (status == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Status found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, status);
        }

        // PUT: api/Statuses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus(int id, Status status)
        {
            if (id != status.Id)
            {
                return BadRequest();
            }

            Status dbStatus = await _statusService.UpdateAsync(status);

            if (dbStatus == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{status.Type} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Statuses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostStatus(Status status)
        {
            var dbStatus = await _statusService.AddAsync(status);

            if (dbStatus == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{status.Type} could not be added.");
            }

            return CreatedAtAction("GetStatus", new { id = status.Id }, status);
        }

        // DELETE: api/Statuses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var statusDelete = await _statusService.GetByIdAsync(id);
            (bool status, string message) = await _statusService.DeleteAsync(statusDelete);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, statusDelete);
        }
    }
}
