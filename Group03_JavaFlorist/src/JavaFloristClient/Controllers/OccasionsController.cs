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
using System.Net.Http.Headers;
using System.Net;

namespace JavaFloristClient.Controllers
{
    public class OccasionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public OccasionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Occasions
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            string token = HttpContext.Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync("api/Occasions");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var occasions = JsonConvert.DeserializeObject<IEnumerable<Occasion>>(content);
                return View(occasions);
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
                return Problem("Unable to fetch occasions from the API.");
            }
        }

        // GET: Occasions/Details/5
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
            var response = await client.GetAsync("api/Occasions/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var occasion = JsonConvert.DeserializeObject<Occasion>(content);
                return View(occasion);
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

        // GET: Occasions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Occasions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Message,Type")] Occasion occasion)
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
                    // Serialize the Occasion object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(occasion), Encoding.UTF8, "application/json");

                    // Send the POST request to the API
                    var response = await client.PostAsync("api/Occasions", jsonContent);

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

            return View(occasion);
        }

        // GET: Occasions/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            var response = await client.GetAsync($"api/Occasions/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var occasion = JsonConvert.DeserializeObject<Occasion>(content);

                if (occasion != null)
                {
                    return View(occasion);
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

        // POST: Occasions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Message,Type")] Occasion occasion)
        {
            if (id != occasion.Id)
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
                    // Serialize the Occasion object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(occasion), Encoding.UTF8, "application/json");

                    // Send the PUT request to the API
                    var response = await client.PutAsync($"api/Occasions/{id}", jsonContent);

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

            return View(occasion);
        }

        // GET: Occasions/Delete/5
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
            var response = await client.GetAsync($"api/Occasions/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var occasion = JsonConvert.DeserializeObject<Occasion>(content);

                if (occasion != null)
                {
                    return View(occasion);
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

        // POST: Occasions/Delete/5
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
                var response = await client.DeleteAsync($"api/Occasions/{id}");

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

            // If the deletion fails, return to the view with the Occasion object for display
            var deletedOccasion = new Occasion { Id = id };
            return View(deletedOccasion);
        }

       
    }
}
