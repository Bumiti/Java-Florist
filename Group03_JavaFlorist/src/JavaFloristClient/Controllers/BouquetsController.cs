using JavaFloristClient.Models;
using JavaFloristClient.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace JavaFloristClient.Controllers
{
    public class BouquetsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJavaFloristClientService _service;

        public BouquetsController(IHttpClientFactory httpClientFactory, IJavaFloristClientService service)
        {
            _httpClientFactory = httpClientFactory;
            _service = service;
        }

        // GET: Bouquets
        public async Task<IActionResult> Index(int? page = 1)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            string token = HttpContext.Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"api/Bouquets?page={page}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bouquets = JsonConvert.DeserializeObject<IEnumerable<Bouquet>>(content);

                var countBouquetUrl = "api/Bouquets/CountBouquet";
                var responseCount = await client.GetAsync(countBouquetUrl);
                var contentCount = await responseCount.Content.ReadAsStringAsync();
                var bouquetsCount = JsonConvert.DeserializeObject<int>(contentCount);

                ViewBag.TotalPageBouquet = bouquetsCount;

                return View(bouquets);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Handle 401 Unauthorized - User is not authenticated to access the API
                // For example, you can redirect the user to a login page or display an error message.
                return RedirectToAction("Login", "Customers"); // Redirect to the login page
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                // Handle 204 No Content - The API returned successfully, but there are no users available.
                return View(new List<Voucher>()); // Return an empty list to the view.
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return View("~/Views/Customers/Forbidden.cshtml");
            }
            else
            {
                return Problem("Unable to fetch data from the API.");
            }
        }

        // GET: Bouquets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient("MyApiClient");
            string token = HttpContext.Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync("api/Bouquets/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bouquet = JsonConvert.DeserializeObject<Bouquet>(content);
                return View(bouquet);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Handle 401 Unauthorized - User is not authenticated to access the API
                // For example, you can redirect the user to a login page or display an error message.
                return RedirectToAction("Login", "Customers"); // Redirect to the login page
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                // Handle 204 No Content - The API returned successfully, but there are no users available.
                return View("~/Views/Customers/bonlebon.cshtml"); // Return an empty list to the view.
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return View("~/Views/Customers/Forbidden.cshtml");
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Bouquets/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");
                string token = HttpContext.Request.Cookies["AccessToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    // Thêm token vào tiêu đề "Authorization"
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                if (string.IsNullOrEmpty(token))
                {
                    // Thêm token vào tiêu đề "Authorization"
                    return RedirectToAction("Login", "Customers"); // Redirect to the login page
                }
                var categories = await _service.GetSelectList<Category>("api/Categories", "Id", "Name");
                var florists = await _service.GetSelectList<Florist>("api/Florists", "Id", "Name");
                // Populate ViewBag with the SelectList data
                ViewBag.CategoryId = categories;
                ViewBag.FloristId = florists;

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
            }

            return View();
        }

        // POST: Bouquets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UnitBrief,UnitPrice,Image,BouquetDate,Available,Description,CategoryId,FloristId,Quantity,Discount,Tag,PriceAfter")] Bouquet bouquet, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");
                    string token = HttpContext.Request.Cookies["AccessToken"];

                    if (!string.IsNullOrEmpty(token))
                    {
                        // Thêm token vào tiêu đề "Authorization"
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    string fileName = Path.GetFileName(file.FileName);
                    string file_path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images/bouquet/", fileName);
                    using (var stream = new FileStream(file_path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    bouquet.Image = "/images/bouquet/" + fileName;

                    bouquet.Discount = Math.Round((bouquet.PriceAfter / bouquet.UnitPrice) * 100, 0);
                    // Serialize the post object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(bouquet), Encoding.UTF8, "application/json");

                    // Send the POST request to the API
                    var response = await client.PostAsync("api/Bouquets", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        // Handle 401 Unauthorized - User is not authenticated to access the API
                        // For example, you can redirect the user to a login page or display an error message.
                        return RedirectToAction("Login", "Customers"); // Redirect to the login page
                    }
                    else if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        // Handle 204 No Content - The API returned successfully, but there are no users available.
                        return View("~/Views/Customers/bonlebon.cshtml"); // Return an empty list to the view.
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        return View("~/Views/Customers/Forbidden.cshtml");
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
            }

            // If there is an error, reload the SelectList and return the view with validation errors
            var categories = await _service.GetSelectList<Category>("api/Categories", "Id", "Name");
            var florists = await _service.GetSelectList<Florist>("api/Florists", "Id", "Name");
            // Populate ViewBag with the SelectList data
            ViewBag.CategoryId = categories;
            ViewBag.FloristId = florists;
            return View(bouquet);
        }

        // GET: Bouquets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MyApiClient");

            string token = HttpContext.Request.Cookies["AccessToken"];
            var categories = await _service.GetSelectList<Category>("api/Categories", "Id", "Name");
            var florists = await _service.GetSelectList<Florist>("api/Florists", "Id", "Name");
            // Populate ViewBag with the SelectList data
            ViewBag.CategoryId = categories;
            ViewBag.FloristId = florists;

            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync($"api/Bouquets/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bouquet = JsonConvert.DeserializeObject<Bouquet>(content);

                if (bouquet != null)
                {
                    return View(bouquet);
                }
                else
                {

                    // Populate ViewBag with the SelectList data
                    ViewBag.CategoryId = categories;
                    ViewBag.FloristId = florists;
                    return NotFound();
                }
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Handle 401 Unauthorized - User is not authenticated to access the API
                // For example, you can redirect the user to a login page or display an error message.
                return RedirectToAction("Login", "Customers"); // Redirect to the login page
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                // Handle 204 No Content - The API returned successfully, but there are no users available.
                return View("~/Views/Customers/bonlebon.cshtml"); // Return an empty list to the view.
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return View("~/Views/Customers/Forbidden.cshtml");
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Bouquets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UnitBrief,UnitPrice,Image,BouquetDate,Available,Description,CategoryId,FloristId,Quantity,Discount,Tag,PriceAfter")] Bouquet bouquet, IFormFile? file)
        {
            if (id != bouquet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");
                    string token = HttpContext.Request.Cookies["AccessToken"];

                    if (!string.IsNullOrEmpty(token))
                    {
                        // Thêm token vào tiêu đề "Authorization"
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    if (file != null)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string file_path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images/bouquet/", fileName);
                        using (var stream = new FileStream(file_path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        bouquet.Image = "/images/bouquet/" + fileName;
                    }
                    else
                    {
                        var responseID = await client.GetAsync($"api/Bouquets/{id}");

                        if (responseID.IsSuccessStatusCode)
                        {
                            var content = await responseID.Content.ReadAsStringAsync();
                            var bouquetOld = JsonConvert.DeserializeObject<Bouquet>(content);
                            string folderPath = "~/images/bouquet/";
                            string imagePath = folderPath + bouquetOld.Image;
                            //string imageUrl = Url.Content(hinhAnh.ImageUrl);
                            bouquet.Image = bouquetOld.Image;
                        }
                    }
                    bouquet.Discount = Math.Round((bouquet.PriceAfter / bouquet.UnitPrice) * 100, 0);
                    // Serialize the category object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(bouquet), Encoding.UTF8, "application/json");

                    // Send the PUT request to the API
                    var response = await client.PutAsync($"api/Bouquets/{id}", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        // Handle 401 Unauthorized - User is not authenticated to access the API
                        // For example, you can redirect the user to a login page or display an error message.
                        return RedirectToAction("Login", "Customers"); // Redirect to the login page
                    }
                    else if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        // Handle 204 No Content - The API returned successfully, but there are no users available.
                        return View("~/Views/Customers/bonlebon.cshtml"); // Return an empty list to the view.
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        return View("~/Views/Customers/Forbidden.cshtml");
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
            }
            var categories = await _service.GetSelectList<Category>("api/Categories", "Id", "Name");
            var florists = await _service.GetSelectList<Florist>("api/Florists", "Id", "Name");
            // Populate ViewBag with the SelectList data
            ViewBag.CategoryId = categories;
            ViewBag.FloristId = florists;
            return View(bouquet);
        }

        // GET: Bouquets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MyApiClient");
            string token = HttpContext.Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync($"api/Bouquets/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bouquet = JsonConvert.DeserializeObject<Bouquet>(content);

                if (bouquet != null)
                {
                    return View(bouquet);
                }
                else
                {
                    return NotFound();
                }
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Handle 401 Unauthorized - User is not authenticated to access the API
                // For example, you can redirect the user to a login page or display an error message.
                return RedirectToAction("Login", "Customers"); // Redirect to the login page
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                // Handle 204 No Content - The API returned successfully, but there are no users available.
                return View("~/Views/Customers/bonlebon.cshtml"); // Return an empty list to the view.
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return View("~/Views/Customers/Forbidden.cshtml");
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Bouquets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");
                string token = HttpContext.Request.Cookies["AccessToken"];

                if (!string.IsNullOrEmpty(token))
                {
                    // Thêm token vào tiêu đề "Authorization"
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                // Send the DELETE request to the API
                var response = await client.DeleteAsync($"api/Bouquets/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Handle 401 Unauthorized - User is not authenticated to access the API
                    // For example, you can redirect the user to a login page or display an error message.
                    return RedirectToAction("Login", "Customers"); // Redirect to the login page
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // Handle 204 No Content - The API returned successfully, but there are no users available.
                    return View("~/Views/Customers/bonlebon.cshtml"); // Return an empty list to the view.
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return View("~/Views/Customers/Forbidden.cshtml");
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

            // If the deletion fails, return to the view with the bouquet object for display
            var deletedBouquet = new Bouquet { Id = id };
            return View(deletedBouquet);
        }
    }
}
