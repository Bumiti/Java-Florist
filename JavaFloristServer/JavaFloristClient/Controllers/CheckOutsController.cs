using JavaFlorist.Models;
using JavaFloristClient.Data;
using JavaFloristClient.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFloristClient.Controllers
{
    public class CheckOutsController : Controller
    {
        private readonly JavaFloristClientContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJavaFloristClientService _service;

        public CheckOutsController(JavaFloristClientContext context, IJavaFloristClientService service, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _service = service;
            _httpClientFactory = httpClientFactory;
        }

        // GET: CheckOuts
        public async Task<IActionResult> Index()
        {
            return _context.CheckOut != null ?
                        View(await _context.CheckOut.ToListAsync()) :
                        Problem("Entity set 'JavaFloristClientContext.CheckOut'  is null.");
        }

        // GET: CheckOuts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CheckOut == null)
            {
                return NotFound();
            }

            var checkOut = await _context.CheckOut
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkOut == null)
            {
                return NotFound();
            }

            return View(checkOut);
        }

        // GET: CheckOuts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CheckOuts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create(CheckOut checkOut)
         {
             try
             {
                 var client = _httpClientFactory.CreateClient("MyApiClient");

                 var responseUser = await client.GetAsync("api/Users/" + 1);
                 var contentUser = await responseUser.Content.ReadAsStringAsync();
                 var user = JsonConvert.DeserializeObject<User>(contentUser);

                 var responseFlorist = await client.GetAsync("api/Florists/" + 1);
                 var contentFlorist = await responseUser.Content.ReadAsStringAsync();
                 var florist = JsonConvert.DeserializeObject<Florist>(contentFlorist);

                 var responseCart = await client.GetAsync("api/Buyers/Cart");
                 var contentCart = await responseCart.Content.ReadAsStringAsync();
                 var item = JsonConvert.DeserializeObject<Cart>(contentCart);

                 // Serialize the post object to JSON
                 // Tạo các đối tượng checkOut, user, florist và item

                 var jsonCheckOut = new StringContent(JsonConvert.SerializeObject(checkOut), Encoding.UTF8, "application/json");
                 var jsonUser = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                 var jsonFlorist = new StringContent(JsonConvert.SerializeObject(florist), Encoding.UTF8, "application/json");
                 var jsonCart = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

                 // Tạo một cấu trúc dữ liệu để chứa tất cả các đối tượng JSON
                 var data = new
                 {
                     CheckOut = checkOut,
                     User = user,
                     Florist = florist,
                     Cart = item
                 };

                 // Chuyển đổi cấu trúc dữ liệu thành JSON
                 var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                 // Send the POST request to the API
                 var response = await client.PostAsync("api/Buyers/Order", jsonContent);

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
             return RedirectToAction(nameof(Index));
         }
 */
        // GET: CheckOuts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CheckOut == null)
            {
                return NotFound();
            }

            var checkOut = await _context.CheckOut.FindAsync(id);
            if (checkOut == null)
            {
                return NotFound();
            }
            return View(checkOut);
        }

        // POST: CheckOuts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SenderFullName,SenderEmail,SenderPhone,SenderAddress,OrderDate,ReciverName,ReciverAddress,ReciverPhone,ReceiverDate,FloristName,FloristLogo,FloristEmail,FloristPhone,FloristAddress,BouquetId,BouquetName,BouquetPrice,BouquetQuantity,BouquetBrief,Messages,Amount,Status")] CheckOut checkOut)
        {
            if (id != checkOut.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checkOut);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckOutExists(checkOut.Id))
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
            return View(checkOut);
        }

        // GET: CheckOuts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CheckOut == null)
            {
                return NotFound();
            }

            var checkOut = await _context.CheckOut
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkOut == null)
            {
                return NotFound();
            }

            return View(checkOut);
        }

        // POST: CheckOuts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CheckOut == null)
            {
                return Problem("Entity set 'JavaFloristClientContext.CheckOut'  is null.");
            }
            var checkOut = await _context.CheckOut.FindAsync(id);
            if (checkOut != null)
            {
                _context.CheckOut.Remove(checkOut);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckOutExists(int id)
        {
            return (_context.CheckOut?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
