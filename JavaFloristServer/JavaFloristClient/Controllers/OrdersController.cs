using JavaFloristClient.Models;
using JavaFloristClient.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace JavaFloristClient.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJavaFloristClientService _service;

        public OrdersController(IHttpClientFactory httpClientFactory, IJavaFloristClientService service)
        {
            _httpClientFactory = httpClientFactory;
            _service = service;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Orders");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<IEnumerable<Order>>(content);
                return View(orders);
            }
            else
            {
                return Problem("Unable to fetch orders from the API.");
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Orders/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(content);
                return View(order);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var florists = await _service.GetSelectList<Florist>("api/Florists", "Id", "Name");
                var receivers = await _service.GetSelectList<Receiver>("api/Receivers", "Id", "Name");
                var statuses = await _service.GetSelectList<Status>("api/Status", "Id", "Type");

                // Populate ViewBag with the SelectList data
                ViewBag.FloristId = florists;
                ViewBag.ReceiverId = receivers;
                ViewBag.StatusId = statuses;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
            }

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,OrderDate,ReceiveDate,ReceiverId,Address,FloristId,BouquetBrief,Messages,Amount,StatusId")] Order order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the post object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

                    // Send the POST request to the API
                    var response = await client.PostAsync("api/Orders", jsonContent);

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
            var florists = await _service.GetSelectList<Florist>("api/Florists", "Id", "Name");
            var receivers = await _service.GetSelectList<Receiver>("api/Receivers", "Id", "Name");
            var statuses = await _service.GetSelectList<Status>("api/Status", "Id", "Type");

            // Populate ViewBag with the SelectList data
            ViewBag.FloristId = florists;
            ViewBag.ReceiverId = receivers;
            ViewBag.StatusId = statuses;
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var florists = await _service.GetSelectList<Florist>("api/Florists", "Id", "Name");
            var receivers = await _service.GetSelectList<Receiver>("api/Receivers", "Id", "Name");
            var statuses = await _service.GetSelectList<Status>("api/Status", "Id", "Type");

            // Populate ViewBag with the SelectList data
            ViewBag.FloristId = florists;
            ViewBag.ReceiverId = receivers;
            ViewBag.StatusId = statuses;
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync($"api/Orders/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(content);

                if (order != null)
                {
                    return View(order);
                }
                else
                {

                    // Populate ViewBag with the SelectList data
                    ViewBag.FloristId = florists;
                    ViewBag.ReceiverId = receivers;
                    ViewBag.StatusId = statuses;
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,OrderDate,ReceiveDate,ReceiverId,Address,FloristId,BouquetBrief,Messages,Amount,StatusId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the category object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

                    // Send the PUT request to the API
                    var response = await client.PutAsync($"api/Orders/{id}", jsonContent);

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
            var florists = await _service.GetSelectList<Florist>("api/Florists", "Id", "Name");
            var receivers = await _service.GetSelectList<Receiver>("api/Receivers", "Id", "Name");
            var statuses = await _service.GetSelectList<Status>("api/Status", "Id", "Type");

            // Populate ViewBag with the SelectList data
            ViewBag.FloristId = florists;
            ViewBag.ReceiverId = receivers;
            ViewBag.StatusId = statuses;
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync($"api/Orders/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(content);

                if (order != null)
                {
                    return View(order);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderConfirm(int? id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                var response = await client.GetAsync("api/Orders/" + id);
                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(content);

                // Serialize the post object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

                // Send the POST request to the API
                var responsePost = await client.PutAsync("api/FloristAlls/OrderConfirm", jsonContent);

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

            return RedirectToAction("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderCancel(int? id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                var response = await client.GetAsync("api/Orders/" + id);
                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(content);

                // Serialize the post object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

                // Send the POST request to the API
                var responsePost = await client.PutAsync("api/FloristAlls/OrderCancel", jsonContent);

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

            return RedirectToAction("Create");
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Send the DELETE request to the API
                var response = await client.DeleteAsync($"api/Orders/{id}");

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

            // If the deletion fails, return to the view with the Order object for display
            var deletedOrder = new Order { Id = id };
            return View(deletedOrder);
        }
    }
}
