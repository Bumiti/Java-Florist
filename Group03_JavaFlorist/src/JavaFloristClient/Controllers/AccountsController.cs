using JavaFloristClient.Models.Accounts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace JavaFloristClient.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

       

        public AccountsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Xóa token từ cookie
            HttpContext.Response.Cookies.Delete("AccessToken");

            // Chuyển hướng đến trang đăng nhập hoặc trang khác tùy chọn
            return RedirectToAction("Login", "Accounts"); // Thay thế bằng đường dẫn trang đăng nhập của bạn
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin user)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Serialize the post object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                // Send the POST request to the API
                var response = await client.PostAsync("api/Auth/login", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content);
                    // Tạo một cookie mới với tên "AccessToken" và giá trị là token nhận được
                    var cookieOptions = new CookieOptions
                    {
                        // Đặt thời gian sống của cookie, ở đây mình đặt là 1 giờ
                        Expires = DateTime.Now.AddHours(1),
                        // Thiết lập trình duyệt chỉ gửi cookie khi yêu cầu gửi đến HTTPS
                        Secure = true,
                        // Chỉ cho phép truy cập cookie thông qua HTTP và không bị truy cập bằng mã JavaScript
                        HttpOnly = true
                    };
                    // Lưu cookie vào response
                    HttpContext.Response.Cookies.Append("AccessToken", content, cookieOptions);
                    return RedirectToAction("DashBoard", "Home");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // If the API request fails, handle the error or display a message to the user
                    // For example, you can read the error response from the API and add an error message to the ModelState
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, "API Error: " + errorContent);
                    return View("~/Views/Accounts/Fobbiden.cshtml");
                }
                return View("~/Views/Accounts/Invalid.cshtml");
            }
            catch (HttpRequestException ex)
            {
                // Handle any exception that occurs during the API call
                ModelState.AddModelError(string.Empty, "API Error: " + ex.Message);
            }
            return View();
        }
        /*public async Task<IActionResult> CallApi(User user)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            //var accessToken = await GetAccessToken(user); // Implement a method to get the access token for the authenticated user
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            var response = await client.GetAsync("api//"); // Replace with your API endpoint URL
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                // Process the API response content as needed
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return View("~/Views/Accounts/Invalid.cshtml");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return View("~/Views/Accounts/Fobbiden.cshtml");
            }
            else
            {
                return View("~/Views/Accounts/Success.cshtml");
            }
            return View();
        }*/

        // Implement a method to get the access token for the authenticated user
        /*private async Task<string> GetAccessToken(User user)
        {*/
        /*[HttpPost]
        private async Task<IActionResult> Login(User user)
        {

            var client = _httpClientFactory.CreateClient("MyApiClient");

            // Serialize the post object to JSON
            var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Send the POST request to the API
            var response = await client.PostAsync("api/Auth/login", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject(content);
                ViewBag.Content = content;
                ViewBag.Token = token;
                return View();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return View();
                //return "Invalid Account";
            }
            else
            {
                return View();
                //return "Unable to fetch data from the API.";
            }
        }*/
    }
}
