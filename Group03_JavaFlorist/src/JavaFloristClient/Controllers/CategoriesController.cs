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
    public class CategoriesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CategoriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            string token = HttpContext.Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync("api/Categories");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(content);
                return View(categories);
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
                return Problem("Unable to fetch categories from the API.");
            }
        }
        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Categories/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categorie = JsonConvert.DeserializeObject<Category>(content);
                return View(categorie);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Categories/Create
        public IActionResult Create()
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
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type")] Category category)
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
                    // Serialize the category object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                    // Send the POST request to the API
                    var response = await client.PostAsync("api/Categories", jsonContent);

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

            return View(category);
        }

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
            var response = await client.GetAsync($"api/Categories/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var category = JsonConvert.DeserializeObject<Category>(content);

                if (category != null)
                {
                    return View(category);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type")] Category category)
        {
            if (id != category.Id)
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
                    // Serialize the category object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                    // Send the PUT request to the API
                    var response = await client.PutAsync($"api/Categories/{id}", jsonContent);

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

            return View(category);
        }
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
            var response = await client.GetAsync($"api/Categories/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var category = JsonConvert.DeserializeObject<Category>(content);

                if (category != null)
                {
                    return View(category);
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
                var response = await client.DeleteAsync($"api/Categories/{id}");

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

            // If the deletion fails, return to the view with the category object for display
            var deletedCategory = new Category { Id = id };
            return View(deletedCategory);
        }
    }
}
