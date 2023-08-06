using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JavaFloristClient.Data;
using JavaFloristClient.Models;
using Newtonsoft.Json;
using System.Text;

namespace JavaFloristClient.Controllers
{
    public class StatusController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public StatusController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Statuses
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Status");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var statuses = JsonConvert.DeserializeObject<IEnumerable<Status>>(content);
                return View(statuses);
            }
            else
            {
                return Problem("Unable to fetch Statuses from the API.");
            }
        }
        // GET: Statuses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Status/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var status = JsonConvert.DeserializeObject<Status>(content);
                return View(status);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Statuses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Statuses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,EntityId,Note")] Status status)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the Status object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(status), Encoding.UTF8, "application/json");

                    // Send the POST request to the API
                    var response = await client.PostAsync("api/Status", jsonContent);

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

            return View(status);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync($"api/Status/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var status = JsonConvert.DeserializeObject<Status>(content);

                if (status != null)
                {
                    return View(status);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,EntityId,Note")] Status status)
        {
            if (id != status.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the Status object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(status), Encoding.UTF8, "application/json");

                    // Send the PUT request to the API
                    var response = await client.PutAsync($"api/Status/{id}", jsonContent);

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

            return View(status);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync($"api/Status/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var status = JsonConvert.DeserializeObject<Status>(content);

                if (status != null)
                {
                    return View(status);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Send the DELETE request to the API
                var response = await client.DeleteAsync($"api/Status/{id}");

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

            // If the deletion fails, return to the view with the Status object for display
            var deletedStatus = new Status { Id = id };
            return View(deletedStatus);
        }
    }
}
