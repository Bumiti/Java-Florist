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
using NuGet.Protocol.Plugins;
using Receiver = JavaFlorist.Models.Receiver;
using Microsoft.AspNetCore.Authorization;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReceiversController : ControllerBase
    {
        private readonly IAllService<Receiver> _receiverService;

        public ReceiversController(IAllService<Receiver> receiverService)
        {
            _receiverService = receiverService;
        }

        // GET: api/Receivers
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReceivers()
        {
            var receivers = await _receiverService.GetAllAsync();

            if (receivers == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No receivers in database");
            }

            return StatusCode(StatusCodes.Status200OK, receivers);
        }

        // GET: api/Receivers/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReceiver(int id)
        {
            Receiver receiver = await _receiverService.GetByIdAsync(id);

            if (receiver == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Receiver found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, receiver);
        }

        // PUT: api/Receivers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutReceiver(int id, Receiver receiver)
        {
            if (id != receiver.Id)
            {
                return BadRequest();
            }

            Receiver dbReceiver = await _receiverService.UpdateAsync(receiver);

            if (dbReceiver == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{receiver.Name} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Receivers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostReceiver(Receiver receiver)
        {
            var dbReceiver = await _receiverService.AddAsync(receiver);

            if (dbReceiver == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{receiver.Name} could not be added.");
            }

            return CreatedAtAction("GetReceiver", new { id = receiver.Id }, receiver);
        }

        // DELETE: api/Receivers/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReceiver(int id)
        {
            var receiver = await _receiverService.GetByIdAsync(id);
            (bool status, string message) = await _receiverService.DeleteAsync(receiver);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, receiver);
        }
    }
}
