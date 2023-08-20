using JavaFloristClient.Data;
using JavaFloristClient.Models;
using JavaFloristClient.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JavaFloristClient.Controllers
{
    public class BuyersController : Controller
    {
        private readonly JavaFloristClientContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJavaFloristClientService _service;

        public BuyersController(JavaFloristClientContext context, IHttpClientFactory httpClientFactory, IJavaFloristClientService service)
        {

            _context = context;
            _httpClientFactory = httpClientFactory;
            _service = service;
        }

        // GET: Buyers
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Buyers/Product");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bouquets = JsonConvert.DeserializeObject<IEnumerable<Bouquet>>(content);
                return View(bouquets);
            }
            else
            {
                return Problem("Unable to fetch bouquets from the API.");
            }

        }

        // GET: Buyers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Buyers/Product/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bouquet = JsonConvert.DeserializeObject<Bouquet>(content);
                return View(bouquet);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Buyers/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id");
            ViewData["FloristId"] = new SelectList(_context.Florist, "Id", "Id");
            return View();
        }

        // POST: Buyers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UnitBrief,UnitPrice,Image,BouquetDate,Available,Description,CategoryId,FloristId,Quantity,Discount,Tag,PriceAfter")] Bouquet bouquet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bouquet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", bouquet.CategoryId);
            ViewData["FloristId"] = new SelectList(_context.Florist, "Id", "Id", bouquet.FloristId);
            return View(bouquet);
        }

        // GET: Buyers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bouquet == null)
            {
                return NotFound();
            }

            var bouquet = await _context.Bouquet.FindAsync(id);
            if (bouquet == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", bouquet.CategoryId);
            ViewData["FloristId"] = new SelectList(_context.Florist, "Id", "Id", bouquet.FloristId);
            return View(bouquet);
        }

        // POST: Buyers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UnitBrief,UnitPrice,Image,BouquetDate,Available,Description,CategoryId,FloristId,Quantity,Discount,Tag,PriceAfter")] Bouquet bouquet)
        {
            if (id != bouquet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bouquet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BouquetExists(bouquet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", bouquet.CategoryId);
            ViewData["FloristId"] = new SelectList(_context.Florist, "Id", "Id", bouquet.FloristId);
            return View(bouquet);
        }

        // GET: Buyers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bouquet == null)
            {
                return NotFound();
            }

            var bouquet = await _context.Bouquet
                .Include(b => b.Category)
                .Include(b => b.Florist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bouquet == null)
            {
                return NotFound();
            }

            return View(bouquet);
        }

        // POST: Buyers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bouquet == null)
            {
                return Problem("Entity set 'JavaFloristClientContext.Bouquet'  is null.");
            }
            var bouquet = await _context.Bouquet.FindAsync(id);
            if (bouquet != null)
            {
                _context.Bouquet.Remove(bouquet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BouquetExists(int id)
        {
            return (_context.Bouquet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
