using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JavaFloristClient.Data;
using JavaFloristClient.Models;
using JavaFloristClient.Repositories;
using Newtonsoft.Json;
using System.Text;
using JavaFloristClient.Repositories;
using JavaFloristClient.Models;

namespace JavaOrderDetailClient.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJavaFloristClientService _service;

        public OrderDetailsController(IHttpClientFactory httpClientFactory, IJavaFloristClientService service)
        {
            _httpClientFactory = httpClientFactory;
            _service = service;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/OrderDetails");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orderDetails = JsonConvert.DeserializeObject<IEnumerable<OrderDetail>>(content);
                return View(orderDetails);
            }
            else
            {
                return Problem("Unable to fetch orderDetails from the API.");
            }
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/OrderDetails/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orderDetail = JsonConvert.DeserializeObject<OrderDetail>(content);
                return View(orderDetail);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: OrderDetails/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var orders = await _service.GetSelectList<Order>("api/Orders", "Id", "UserId");
                var bouquets = await _service.GetSelectList<Bouquet>("api/Bouquets", "Id", "Name");

                // Populate ViewBag with the SelectList data
                ViewBag.OrderId = orders;
                ViewBag.BouquetId = bouquets;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
            }

            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderId,BouquetId,UnitPrice,Quantity,Discount")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the post object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(orderDetail), Encoding.UTF8, "application/json");

                    // Send the POST request to the API
                    var response = await client.PostAsync("api/OrderDetails", jsonContent);

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
            var orders = await _service.GetSelectList<Order>("api/Orders", "Id", "UserId");
            var bouquets = await _service.GetSelectList<Bouquet>("api/Bouquets", "Id", "Name");

            // Populate ViewBag with the SelectList data
            ViewBag.OrderId = orders;
            ViewBag.BouquetId = bouquets;
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var orders = await _service.GetSelectList<Order>("api/Orders", "Id", "UserId");
            var bouquets = await _service.GetSelectList<Bouquet>("api/Bouquets", "Id", "Name");

            // Populate ViewBag with the SelectList data
            ViewBag.OrderId = orders;
            ViewBag.BouquetId = bouquets;
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync($"api/OrderDetails/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orderDetail = JsonConvert.DeserializeObject<OrderDetail>(content);

                if (orderDetail != null)
                {
                    return View(orderDetail);
                }
                else
                {

                    // Populate ViewBag with the SelectList data
                    ViewBag.OrderId = orders;
                    ViewBag.BouquetId = bouquets;
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,BouquetId,UnitPrice,Quantity,Discount")] OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the category object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(orderDetail), Encoding.UTF8, "application/json");

                    // Send the PUT request to the API
                    var response = await client.PutAsync($"api/OrderDetails/{id}", jsonContent);

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
            var orders = await _service.GetSelectList<Order>("api/Orders", "Id", "UserId");
            var bouquets = await _service.GetSelectList<Bouquet>("api/Bouquets", "Id", "Name");

            // Populate ViewBag with the SelectList data
            ViewBag.OrderId = orders;
            ViewBag.BouquetId = bouquets;
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync($"api/OrderDetails/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orderDetail = JsonConvert.DeserializeObject<OrderDetail>(content);

                if (orderDetail != null)
                {
                    return View(orderDetail);
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

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Send the DELETE request to the API
                var response = await client.DeleteAsync($"api/OrderDetails/{id}");

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

            // If the deletion fails, return to the view with the OrderDetail object for display
            var deletedOrderDetail = new OrderDetail { Id = id };
            return View(deletedOrderDetail);
        }
    }
}
