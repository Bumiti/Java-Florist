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
    public class BouquetsController : ControllerBase
    {
        private readonly IAllService<Bouquet> _bouquetservice;

        public BouquetsController(IAllService<Bouquet> bouquetService)
        {
            _bouquetservice = bouquetService;
        }

        // GET: api/Bouquets
        [HttpGet]
        public async Task<IActionResult> GetBouquets()
        {
            var bouquets = await _bouquetservice.GetAllAsync();

            if (bouquets == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, bouquets);
        }

        // GET: api/Bouquets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBouquet(int id, bool includes = true)
        {
            Bouquet bouquet = await _bouquetservice.GetByIdAsync(id, includes);

            if (bouquet == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Bouquet found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, bouquet);
        }

        // PUT: api/Bouquets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBouquet(int id, Bouquet bouquet)
        {
            if (id != bouquet.Id)
            {
                return BadRequest();
            }

            Bouquet dbBouquet = await _bouquetservice.UpdateAsync(bouquet);

            if (dbBouquet == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{bouquet.Name} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Bouquets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostBouquet(Bouquet bouquet)
        {
            var dbBouquet = await _bouquetservice.AddAsync(bouquet);

            if (dbBouquet == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{bouquet.Name} could not be added.");
            }

            return CreatedAtAction("GetBouquet", new { id = bouquet.Id }, bouquet);
        }

        // DELETE: api/Bouquets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBouquet(int id)
        {
            var bouquet = await _bouquetservice.GetByIdAsync(id, false);
            (bool status, string message) = await _bouquetservice.DeleteAsync(bouquet);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, bouquet);
        }
    }
}
