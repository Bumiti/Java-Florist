using JavaFloristClient.Data;
using JavaFloristClient.Models;
using JavaFloristClient.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace JavaFloristClient.Controllers
{
    public class CartsController : Controller
    {
        private readonly JavaFloristClientContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJavaFloristClientService _service;
        public CartsController(JavaFloristClientContext context, IJavaFloristClientService service, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _service = service;
            _httpClientFactory = httpClientFactory;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Buyers/Cart");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cart = JsonConvert.DeserializeObject<IEnumerable<Cart>>(content);
                return View(cart);
            }
            else
            {
                return Problem("Unable to fetch cart from the API.");
            }
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        /*public async Task<IActionResult> Create()
        {
            return View();
        }*/
        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                var response = await client.GetAsync("api/Buyers/Product/" + id);
                var content = await response.Content.ReadAsStringAsync();
                var bouquet = JsonConvert.DeserializeObject<Bouquet>(content);

                // Serialize the post object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(bouquet), Encoding.UTF8, "application/json");

                // Send the POST request to the API
                var responsePost = await client.PostAsync("api/Buyers/Cart", jsonContent);

                if (responsePost.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // If the API request fails, handle the error or display a message to the user
                    // For example, you can read the error response from the API and add an error message to the ModelState
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, "API Error: " + errorContent);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any exception that occurs during the API call
                ModelState.AddModelError(string.Empty, "API Error: " + ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit()
        {
            return View();
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Price,Quantity")] Cart cart)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Serialize the category object to JSON
                var data = new { Id = id };
                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent requestContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the PUT request to the API
                var response = await client.PutAsync($"api/Buyers/Cart/{id}", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // If the API request fails, handle the error or display a message to the user
                    // For example, you can read the error response from the API and add an error message to the ModelState
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, "API Error: " + errorContent);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any exception that occurs during the API call
                ModelState.AddModelError(string.Empty, "API Error: " + ex.Message);
            }
            return View();
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync($"api/Buyers/Cart/{id}");

            if (response.IsSuccessStatusCode)
            {
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'JavaFloristClientContext.Cart'  is null.");
            }
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int? id)
        {
            return (_context.Cart?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
