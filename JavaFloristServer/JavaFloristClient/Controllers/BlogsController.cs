using JavaFloristClient.Models;
using JavaFloristClient.Models.Accounts;
using JavaFloristClient.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace JavaFloristClient.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJavaFloristClientService _service;


        public BlogsController(IHttpClientFactory httpClientFactory, IJavaFloristClientService service)
        {
            _httpClientFactory = httpClientFactory;
            _service = service;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Blogs");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var blogs = JsonConvert.DeserializeObject<IEnumerable<Blog>>(content);
                return View(blogs);
            }
            else
            {
                return Problem("Unable to fetch blogs from the API.");
            }
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Blogs/" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var blog = JsonConvert.DeserializeObject<Blog>(content);
                return View(blog);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Blogs/Create
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

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,PublishDate,UserId,StatusId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the post object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(blog), Encoding.UTF8, "application/json");

                    // Send the POST request to the API
                    var response = await client.PostAsync("api/Blogs", jsonContent);

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
            return View(blog);
        }

        // GET: Blogs/Edit/5
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
            var response = await client.GetAsync($"api/Blogs/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var blog = JsonConvert.DeserializeObject<Blog>(content);

                if (blog != null)
                {
                    return View(blog);
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

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,PublishDate,UserId,StatusId")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    // Serialize the category object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(blog), Encoding.UTF8, "application/json");

                    // Send the PUT request to the API
                    var response = await client.PutAsync($"api/Blogs/{id}", jsonContent);

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
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int? id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                var response = await client.GetAsync("api/Blogs/" + id);
                var content = await response.Content.ReadAsStringAsync();
                var blog = JsonConvert.DeserializeObject<Blog>(content);

                // Serialize the post object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(blog), Encoding.UTF8, "application/json");

                // Send the POST request to the API
                var responsePost = await client.PutAsync("api/Auth/verify-blog", jsonContent);

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

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync($"api/Blogs/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var blog = JsonConvert.DeserializeObject<Blog>(content);

                if (blog != null)
                {
                    return View(blog);
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

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Send the DELETE request to the API
                var response = await client.DeleteAsync($"api/Blogs/{id}");

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

            // If the deletion fails, return to the view with the blog object for display
            var deletedBlog = new Blog { Id = id };
            return View(deletedBlog);
        }
    }
}
