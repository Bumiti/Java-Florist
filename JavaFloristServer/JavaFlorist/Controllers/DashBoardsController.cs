using JavaFlorist.Repositories.IServices;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardsController : ControllerBase
    {
        private readonly IDashBoard _dashBoard;

        public DashBoardsController(IDashBoard dashboard)
        {
            _dashBoard = dashboard;
        }
        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            var cartItems = await _dashBoard.GetFilterByFlorists();

            if (cartItems == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Bouquets in database");
            }

            return StatusCode(StatusCodes.Status200OK, cartItems);
        }
    }
}
