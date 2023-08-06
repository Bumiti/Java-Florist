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
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Bouquets");

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

        // GET: Bouquets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Bouquets/" + id);

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

        // GET: Bouquets/Create
        public async Task<IActionResult> Create()
        {
            try
            {
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
        public async Task<IActionResult> Create([Bind("Id,Name,UnitBrief,UnitPrice,Image,BouquetDate,Available,Description,CategoryId,FloristId,Quantity,Discount,Tag,PriceAfter")] Bouquet bouquet)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the post object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(bouquet), Encoding.UTF8, "application/json");

                    // Send the POST request to the API
                    var response = await client.PostAsync("api/Bouquets", jsonContent);

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
            var categories = await _service.GetSelectList<Category>("api/Categories", "Id", "Name");
            var florists = await _service.GetSelectList<Florist>("api/Florists", "Id", "Name");
            // Populate ViewBag with the SelectList data
            ViewBag.CategoryId = categories;
            ViewBag.FloristId = florists;
            var client = _httpClientFactory.CreateClient("MyApiClient");
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
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the category object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(bouquet), Encoding.UTF8, "application/json");

                    // Send the PUT request to the API
                    var response = await client.PutAsync($"api/Bouquets/{id}", jsonContent);

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

                // Send the DELETE request to the API
                var response = await client.DeleteAsync($"api/Bouquets/{id}");

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

            // If the deletion fails, return to the view with the bouquet object for display
            var deletedBouquet = new Bouquet { Id = id };
            return View(deletedBouquet);
        }
    }
}
