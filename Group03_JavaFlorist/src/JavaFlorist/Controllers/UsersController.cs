using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    //[Authorize(Roles = "Admin,Florist,User")]
    public class UsersController : ControllerBase
    {
        private readonly IAllService<User> _userService;

        public UsersController(IAllService<User> userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet/*, Authorize(Roles = "Admin")*/]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllAsync();

            if (users == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Users in database");
            }


            return StatusCode(StatusCodes.Status200OK, users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(int id, bool includes = true)
        {
            User user = await _userService.GetByIdAsync(id, includes);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No User found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            /*if (id != user.Id)
            {
                return BadRequest();
            }*/

            User dbUser = await _userService.UpdateAsync(user);

            if (dbUser == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{user.FullName} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostUser(User user)
        {
            var dbUser = await _userService.AddAsync(user);

            if (dbUser == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{user.FullName} could not be added.");
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetByIdAsync(id, false);
            (bool status, string message) = await _userService.DeleteAsync(user);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, user);
        }
    }
}
