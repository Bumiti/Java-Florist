using JavaFloristClient.Models;
using JavaFloristClient.Models.Accounts;
using JavaFloristClient.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace JavaFloristClient.Controllers
{
    public class FloristsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJavaFloristClientService _service;

        public FloristsController(IHttpClientFactory httpClientFactory, IJavaFloristClientService service)
        {
            _httpClientFactory = httpClientFactory;
            _service = service;
        }

        // GET: Florists
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Florists");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var florists = JsonConvert.DeserializeObject<IEnumerable<Florist>>(content);
                return View(florists);
            }
            else
            {
                return Problem("Unable to fetch florists from the API.");
            }
        }

        // GET: Florists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Florists/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var florist = JsonConvert.DeserializeObject<Florist>(content);
                return View(florist);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Florists/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var users = await _service.GetSelectList<User>("api/Users", "Id", "FullName");
                var statuses = await _service.GetSelectList<Status>("api/Status", "Id", "Type");

                // Populate ViewBag with the SelectList data
                ViewBag.UserId = users;
                ViewBag.StatusId = statuses;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
            }

            return View();
        }

        // POST: Florists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Logo,Email,Phone,Address,UserId,StatusId")] Florist florist)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the post object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(florist), Encoding.UTF8, "application/json");
                    string token = HttpContext.Request.Cookies["AccessToken"];

                    if (!string.IsNullOrEmpty(token))
                    {
                        // Thêm token vào tiêu đề "Authorization"
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    // Send the POST request to the API
                    //var response = await client.PostAsync("api/Florists", jsonContent);
                    var response = await client.PostAsync("api/Auth/signUpFlorist", jsonContent);
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
            }

            // If there is an error, reload the SelectList and return the view with validation errors
            var users = await _service.GetSelectList<User>("api/Users", "Id", "FullName");
            var statuses = await _service.GetSelectList<Status>("api/Status", "Id", "Type");

            // Populate ViewBag with the SelectList data
            ViewBag.UserId = users;
            ViewBag.StatusId = statuses;
            return View(florist);
        }

        // GET: Florists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var users = await _service.GetSelectList<User>("api/Users", "Id", "FullName");
            var statuses = await _service.GetSelectList<Status>("api/Status", "Id", "Type");

            // Populate ViewBag with the SelectList data
            ViewBag.UserId = users;
            ViewBag.StatusId = statuses;
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync($"api/Florists/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var florist = JsonConvert.DeserializeObject<Florist>(content);

                if (florist != null)
                {
                    return View(florist);
                }
                else
                {

                    // Populate ViewBag with the SelectList data
                    ViewBag.UserId = users;
                    ViewBag.StatusId = statuses;
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Florists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Logo,Email,Phone,Address,UserId,StatusId")] Florist florist)
        {
            if (id != florist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the category object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(florist), Encoding.UTF8, "application/json");

                    // Send the PUT request to the API
                    var response = await client.PutAsync($"api/Florists/{id}", jsonContent);

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
            }
            var users = await _service.GetSelectList<User>("api/Users", "Id", "FullName");
            var statuses = await _service.GetSelectList<Status>("api/Status", "Id", "Type");

            // Populate ViewBag with the SelectList data
            ViewBag.UserId = users;
            ViewBag.StatusId = statuses;
            return View(florist);
        }

        // GET: Florists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync($"api/Florists/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var florist = JsonConvert.DeserializeObject<Florist>(content);

                if (florist != null)
                {
                    return View(florist);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Florists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Send the DELETE request to the API
                var response = await client.DeleteAsync($"api/Florists/{id}");

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

            // If the deletion fails, return to the view with the Florist object for display
            var deletedFlorist = new Florist { Id = id };
            return View(deletedFlorist);
        }
    }
}
