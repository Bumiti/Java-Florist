using JavaFloristClient.Data;
using JavaFloristClient.Models;
using JavaFloristClient.Models.Accounts;
using JavaFloristClient.Repositories;
using JavaFloristClient.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace JavaFloristClient.Controllers
{
    public class CustomersController : Controller
    {
        private readonly JavaFloristClientContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJavaFloristClientService _service;
        private readonly IVnPayService _vnPayService;

        public CustomersController(JavaFloristClientContext context, IHttpClientFactory httpClientFactory, IJavaFloristClientService service)
        {

            _context = context;
            _httpClientFactory = httpClientFactory;
            _service = service;
        }

        /*
                public async Task<IActionResult> Cart()
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");
                    IEnumerable<Cart> cart;

                    // Kiểm tra xem có dữ liệu giỏ hàng trong cookie không
                    if (Request.Cookies.TryGetValue("Cart", out var cartJson))
                    {
                        cart = JsonConvert.DeserializeObject<IEnumerable<Cart>>(cartJson);
                    }
                    else
                    {
                        // Gọi API để lấy dữ liệu giỏ hàng
                        var response = await client.GetAsync("api/Buyers/Cart");

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            cart = JsonConvert.DeserializeObject<IEnumerable<Cart>>(content);
                        }
                        else
                        {
                            return Problem("Unable to fetch cart from the API.");
                        }
                    }

                    SetupCommonData();
                    return View(cart);
                }
                public async Task<IActionResult> ClearCart()
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");
                    var response = await client.GetAsync("api/Buyers/ClearCart");

                    if (response.IsSuccessStatusCode)
                    {
                        SetupCommonData();
                        Response.Cookies.Delete("Cart");
                        return RedirectToAction("Cart");
                    }
                    else
                    {
                        return Problem("Unable to fetch cart from the API.");
                    }
                }

                [HttpPost]
                public async Task<IActionResult> AddToCart(int id)
                {
                    try
                    {
                        var client = _httpClientFactory.CreateClient("MyApiClient");

                        var response = await client.GetAsync("api/Buyers/Product/" + id);
                        var content = await response.Content.ReadAsStringAsync();
                        var bouquet = JsonConvert.DeserializeObject<Bouquet>(content);

                        // Serialize the post object to JSON
                        var jsonContent = new StringContent(JsonConvert.SerializeObject(bouquet), Encoding.UTF8, "application/json");

                        // Send the POST request to the API
                        var responsePost = await client.PostAsync("api/Buyers/Cart", jsonContent);

                        if (responsePost.IsSuccessStatusCode)
                        {
                            // Lưu giỏ hàng vào cookie
                            var cartJson = JsonConvert.SerializeObject(bouquet);
                            var cookieOptions = new CookieOptions
                            {
                                Expires = DateTime.UtcNow.AddDays(7), // Thời gian hết hạn của cookie
                                SameSite = SameSiteMode.None, // Tùy chọn SameSite
                                Secure = true, // Yêu cầu giao tiếp qua HTTPS
                            };
                            Response.Cookies.Append("Cart", cartJson, cookieOptions);
                            return RedirectToAction("Cart");
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
                    return RedirectToAction(nameof(Index));
                }
                [HttpPost]
                public async Task<IActionResult> UpdateCart(int id, int quantity)
                {
                    var client = _httpClientFactory.CreateClient("MyApiClient");

                    var response = await client.GetAsync("api/Buyers/Cart/" + id);
                    var content = await response.Content.ReadAsStringAsync();
                    var cartItem = JsonConvert.DeserializeObject<Cart>(content);

                    // Update the quantity
                    cartItem.Quantity = quantity;

                    // Serialize the updated cart item to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(cartItem), Encoding.UTF8, "application/json");

                    // Send the PUT request to update the cart item in the API
                    var responsePut = await client.PutAsync("api/Buyers/Cart", jsonContent);

                    if (responsePut.IsSuccessStatusCode)
                    {
                        var updatedCartJson = JsonConvert.SerializeObject(cartItem);
                        var cookieOptions = new CookieOptions
                        {
                            Expires = DateTime.UtcNow.AddDays(7),
                            SameSite = SameSiteMode.None,
                            Secure = true,
                        };
                        Response.Cookies.Append("Cart", updatedCartJson, cookieOptions);
                        return RedirectToAction("Cart");
                    }
                    else
                    {
                        // Handle API error
                        var errorContent = await responsePut.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, "API Error: " + errorContent);
                    }

                    return RedirectToAction("Cart");
                }

                [HttpGet]
                public async Task<IActionResult> DeleteCart(int id)
                {
                    try
                    {
                        var client = _httpClientFactory.CreateClient("MyApiClient");

                        // Send the DELETE request to the API
                        var response = await client.DeleteAsync($"api/Buyers/Cart/{id}");

                        if (response.IsSuccessStatusCode)
                        {
                            if (Request.Cookies.TryGetValue("Cart", out var cartJson))
                            {
                                var existingCart = JsonConvert.DeserializeObject<List<Cart>>(cartJson);

                                // Xóa mục khỏi dữ liệu giỏ hàng trong cookie
                                var updatedCart = existingCart.Where(item => item.Id != id).ToList();
                                var updatedCartJson = JsonConvert.SerializeObject(updatedCart);
                                var cookieOptions = new CookieOptions
                                {
                                    Expires = DateTime.UtcNow.AddDays(7),
                                    SameSite = SameSiteMode.None,
                                    Secure = true,
                                };
                                Response.Cookies.Append("Cart", updatedCartJson, cookieOptions);
                            }
                            return RedirectToAction("Cart");
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
                    return RedirectToAction("Cart");
                }
        */


        public async void SetupCommonData()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            string token = HttpContext.Request.Cookies["AccessToken"];
            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                // Assuming you have the 'jwt' variable containing the JWT token string
                // Extract the email claim
                var userEmail = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
                ViewBag.FullName = userEmail;
            }
            // Decode the token to get the email
            var responseCount = await client.GetAsync("api/Buyers/CountCart");
            var contentCount = await responseCount.Content.ReadAsStringAsync();
            var cartCount = JsonConvert.DeserializeObject<int>(contentCount);


            ViewBag.Cart = cartCount;
        }
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            string token = HttpContext.Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync("api/Buyers/Index");
            SetupCommonData();


            if (response.IsSuccessStatusCode)
            {
                SetupCommonData();
                var content = await response.Content.ReadAsStringAsync();
                var bouquets = JsonConvert.DeserializeObject<IEnumerable<Bouquet>>(content);
                return View(bouquets);
            }
            else
            {
                return Problem("Unable to fetch bouquets from the API.");
            }
        }
        public async Task<IActionResult> Blog()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Buyers/Blog");

            if (response.IsSuccessStatusCode)
            {
                SetupCommonData();
                var content = await response.Content.ReadAsStringAsync();
                var blogs = JsonConvert.DeserializeObject<IEnumerable<Blog>>(content);
                return View(blogs);
            }
            else
            {
                return Problem("Unable to fetch bouquets from the API.");
            }
        }
        public async Task<IActionResult> BlogDetails(int id)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            var response = await client.GetAsync($"api/Buyers/Blog/{id}");

            if (response.IsSuccessStatusCode)
            {
                SetupCommonData();
                var content = await response.Content.ReadAsStringAsync();
                var orderDetails = JsonConvert.DeserializeObject<Blog>(content);
                return View(orderDetails);
            }
            else
            {
                return Problem("Unable to fetch OrderDetails from the API.");
            }
        }
        public async Task<IActionResult> MyOrder()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            string token = HttpContext.Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Customers"); // Redirect to the login page
            }
            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync("api/Buyers/Order");

            if (response.IsSuccessStatusCode)
            {
                SetupCommonData();
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<IEnumerable<Order>>(content);
                return View(orders);
            }
            else
            {
                return Problem("Unable to fetch order from the API.");
            }
        }
        public async Task<IActionResult> OrderDetails(int id)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            string token = HttpContext.Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Customers"); // Redirect to the login page
            }
            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"api/Buyers/OrderDetails/{id}");

            if (response.IsSuccessStatusCode)
            {
                SetupCommonData();
                var content = await response.Content.ReadAsStringAsync();
                var orderDetails = JsonConvert.DeserializeObject<IEnumerable<OrderDetail>>(content);
                return View(orderDetails);
            }
            else
            {
                return Problem("Unable to fetch OrderDetails from the API.");
            }
        }

        public async Task<IActionResult> Shop(int page = 1, string search = null, double? from = null, double? to = null, string sortBy = null, string? occasion = null)
        {
            //dungChuan
            var client = _httpClientFactory.CreateClient("MyApiClient");
            // Construct the API endpoint URL with query parameters
            var apiUrl = $"api/Buyers/Product?page={page}";
            if (!string.IsNullOrEmpty(search))
            {
                apiUrl += $"&search={search}";
            }
            if (from.HasValue)
            {
                apiUrl += $"&from={from}";
            }
            if (to.HasValue)
            {
                apiUrl += $"&to={to}";
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                apiUrl += $"&sortBy={sortBy}";
            }
            if (occasion != null)
            {
                apiUrl += $"&occasion={occasion}";
            }
            /*var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Buyers/Index");

            if (response.IsSuccessStatusCode)
            {
                SetupCommonData();
                var content = await response.Content.ReadAsStringAsync();
                var bouquets = JsonConvert.DeserializeObject<IEnumerable<Bouquet>>(content);
                return View(bouquets);
            }
            else
            {
                return Problem("Unable to fetch bouquets from the API.");
            }*/

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                SetupCommonData();
                var content = await response.Content.ReadAsStringAsync();
                var bouquets = JsonConvert.DeserializeObject<IEnumerable<Bouquet>>(content);
                // Chú ý rằng đây là List<Bouquet> thay vì IEnumerable<Bouquet>

                var countBouquetUrl = $"api/Buyers/CountBouquet/";
                var responseCount = await client.GetAsync(countBouquetUrl);
                var contentCount = await responseCount.Content.ReadAsStringAsync();
                var bouquetsCount = JsonConvert.DeserializeObject<int>(contentCount);
                ViewBag.TotalPage = bouquetsCount;

                return View(bouquets);
            }
            else
            {
                return Problem("Unable to fetch bouquets from the API.");
            }
        }

        /*public async Task<IActionResult> Shop(int page = 1, string search = null, double? from = null, double? to = null, string sortBy = null)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            // Construct the API endpoint URL with query parameters
            var apiUrl = $"api/Buyers/Product?page={page}";
            if (!string.IsNullOrEmpty(search))
            {
                apiUrl += $"&search={search}";
            }
            if (from.HasValue)
            {
                apiUrl += $"&from={from}";
            }
            if (to.HasValue)
            {
                apiUrl += $"&to={to}";
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                apiUrl += $"&sortBy={sortBy}";
            }

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bouquets = JsonConvert.DeserializeObject<List<Bouquet>>(content);

                var pageIndex = page; // Use the provided page parameter
                var pageSize = 9;
                var paginatedList = PaginatedList<Bouquet>.Create(bouquets.AsQueryable(), pageIndex, pageSize);

                return View(paginatedList);
            }
            else
            {
                return Problem("Unable to fetch bouquets from the API.");
            }
        }*/


        public IActionResult About()
        {
            SetupCommonData();
            return View();
        }
        public async Task<IActionResult> Cart()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Buyers/Cart");

            if (response.IsSuccessStatusCode)
            {
                SetupCommonData();
                var content = await response.Content.ReadAsStringAsync();
                var cart = JsonConvert.DeserializeObject<IEnumerable<Cart>>(content);
                return View(cart);
            }
            else
            {
                return Problem("Unable to fetch cart from the API.");
            }
        }
        public async Task<IActionResult> ClearCart()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Buyers/ClearCart");

            if (response.IsSuccessStatusCode)
            {
                SetupCommonData();

                return RedirectToAction("Cart");
            }
            else
            {
                return Problem("Unable to fetch cart from the API.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                var response = await client.GetAsync("api/Buyers/Product/" + id);
                var content = await response.Content.ReadAsStringAsync();
                var bouquet = JsonConvert.DeserializeObject<Bouquet>(content);

                // Serialize the post object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(bouquet), Encoding.UTF8, "application/json");

                // Send the POST request to the API
                var responsePost = await client.PostAsync("api/Buyers/Cart", jsonContent);

                if (responsePost.IsSuccessStatusCode)
                {
                    return RedirectToAction("Cart");
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
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCart(int id, int quantity)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            var response = await client.GetAsync("api/Buyers/Cart/" + id);
            var content = await response.Content.ReadAsStringAsync();
            var cartItem = JsonConvert.DeserializeObject<Cart>(content);

            // Update the quantity
            cartItem.Quantity = quantity;

            // Serialize the updated cart item to JSON
            var jsonContent = new StringContent(JsonConvert.SerializeObject(cartItem), Encoding.UTF8, "application/json");

            // Send the PUT request to update the cart item in the API
            var responsePut = await client.PutAsync("api/Buyers/Cart", jsonContent);

            if (responsePut.IsSuccessStatusCode)
            {
                return RedirectToAction("Cart");
            }
            else
            {
                // Handle API error
                var errorContent = await responsePut.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "API Error: " + errorContent);
            }

            return RedirectToAction("Cart");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCart(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Send the DELETE request to the API
                var response = await client.DeleteAsync($"api/Buyers/Cart/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Cart");
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
            return RedirectToAction("Cart");
        }

        public IActionResult Contact()
        {
            SetupCommonData();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CheckOut(int? occasion)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            string token = HttpContext.Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Customers"); // Redirect to the login page
            }
            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }



            var response = await client.GetAsync("api/Buyers/Cart");
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return View("~/Views/Customers/Invalid.cshtml");
            }

            // Decode the token to get the email
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            // Assuming you have the 'jwt' variable containing the JWT token string

            // Extract the email claim
            var userEmail = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;



            if (!string.IsNullOrEmpty(userEmail))
            {
                var responseUser = await client.GetAsync($"api/Auth/{userEmail}");
                // Process the user's email



                if (response.IsSuccessStatusCode)
                {
                    SetupCommonData();
                    var content = await response.Content.ReadAsStringAsync();
                    var contentUser = await responseUser.Content.ReadAsStringAsync();
                    var cart = JsonConvert.DeserializeObject<IEnumerable<Cart>>(content).ToList();
                    var user = JsonConvert.DeserializeObject<User>(contentUser);

                    var viewModel = new CheckOut
                    {
                        CartItems = new List<Cart>() // Tạo một bản sao của danh sách hiện tại
                    };

                    viewModel.CartItems.AddRange(cart); // Thêm các mục mới vào danh sách

                    viewModel.SenderFullName = user.FullName;
                    viewModel.SenderEmail = user.Email;
                    viewModel.SenderPhone = user.Phone;
                    viewModel.SenderAddress = user.Address;

                    /*viewModel.ReciverName = "Nghiem Kha An";
                    viewModel.ReciverPhone = "123456789";
                    viewModel.ReciverAddress = "123 Nguyen Thai Binh";*/
                    viewModel.ReceiverDate = viewModel.OrderDate?.AddHours(5);

                    ViewBag.Occasion = "Select an occasion";
                    var occasions = await _service.GetSelectList<Occasion>("api/Occasions", "Id", "Type");
                    if (occasion != null)
                    {
                        var message = await _service.GetSelectList<Occasion>($"api/Buyers/Mess/{occasion}", "Id", "Message");
                        ViewBag.Message = message;
                        if (occasion == 1)
                        {
                            ViewBag.Occasion = "Birthday";
                        }
                        else if (occasion == 2)
                        {
                            ViewBag.Occasion = "Get Well";
                        }
                        else if (occasion == 3)
                        {
                            ViewBag.Occasion = "Graduation";
                        }
                        else if (occasion == 4)
                        {
                            ViewBag.Occasion = "Grand Opening";
                        }
                        else if (occasion == 5)
                        {
                            ViewBag.Occasion = "Love";
                        }
                        else if (occasion == 6)
                        {
                            ViewBag.Occasion = "Noel";
                        }
                        else if (occasion == 7)
                        {
                            ViewBag.Occasion = "Sympathy - Funerals";
                        }
                        else if (occasion == 8)
                        {
                            ViewBag.Occasion = "Thanks";
                        }
                        else if (occasion == 9)
                        {
                            ViewBag.Occasion = "Wedding";
                        }
                        else
                        {
                            // Handle the default case if needed
                            throw new ArgumentOutOfRangeException(nameof(occasion), "Invalid occasion value");
                        }
                    }
                    // Populate ViewBag with the SelectList data

                    var orderDateMessage = TempData["OrderDate"] as string;
                    ViewBag.CheckPayment = HttpContext.Session.GetString("CheckOut");

                    ViewBag.OrderDate = orderDateMessage;
                    return View("CheckOut", viewModel);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // Handle 401 Unauthorized - User is not authenticated to access the API
                    // For example, you can redirect the user to a login page or display an error message.
                    return RedirectToAction("Login", "Customers"); // Redirect to the login page
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    SetupCommonData();
                    return View("~/Views/Accounts/Fobbiden.cshtml");
                }
                else
                {
                    // Handle other error status codes
                    // For example, you can log the error or display a generic error message.
                    return Problem("Unable to fetch users from the API.");
                }
            }
            return RedirectToAction("Login", "Customers"); // Redirect to the login page
        }


        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckOut checkOut)
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

                // Fetch user information from the API
                var userResponse = await client.GetAsync("api/Auth"); // Adjust the API endpoint accordingly

                if (userResponse.IsSuccessStatusCode)
                {
                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    //var user = JsonConvert.DeserializeObject<string>(userJson); // Replace UserModel with your user model class

                    // Modify the checkOut object with user information

                    // Serialize the modified checkOut object to JSON

                    // Serialize the Receiver object to JSON
                    if (checkOut.OrderDate == null)
                    {
                        TempData["OrderDate"] = "OrderDate cannot be null";
                        return RedirectToAction("CheckOut");
                    }
                    // Tính khoảng thời gian đến 16 giờ (16:00)

                    TimeSpan startWorkingHour = new TimeSpan(9, 0, 0);
                    TimeSpan endWorkingHour = new TimeSpan(21, 0, 0);
                    TimeSpan timeUntil5Hours = new TimeSpan(5, 0, 0);

                    if (checkOut.OrderDate.HasValue)
                    {
                        DateTime orderDate = checkOut.OrderDate.Value;

                        if (orderDate.TimeOfDay >= endWorkingHour - timeUntil5Hours)
                        {
                            // Cộng 1 ngày và đặt giờ là 9 giờ sáng
                            checkOut.ReceiverDate = orderDate.Date.AddDays(1).Add(startWorkingHour);
                        }
                        else if (orderDate.TimeOfDay >= startWorkingHour)
                        {
                            // Cộng 5 giờ để được ReceiverDate
                            checkOut.ReceiverDate = orderDate.Add(timeUntil5Hours);
                        }
                        else
                        {
                            // Nếu đặt trước giờ làm việc, đặt thời gian giao là vào buổi sáng của ngày đó
                            checkOut.ReceiverDate = orderDate.Date.Add(startWorkingHour);
                        }
                    }

                    if (checkOut.ShipToMyAddress)
                    {
                        checkOut.ReciverName = checkOut.SenderFullName;
                        checkOut.ReciverPhone = checkOut.SenderPhone;
                        checkOut.ReciverAddress = checkOut.SenderAddress;
                        checkOut.Messages = 1;
                    }

                    // Send the POST request to the API
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(checkOut), Encoding.UTF8, "application/json");

                    // Send the POST request to the API
                    var response = await client.PostAsync("api/Buyers/Order", jsonContent);
                    var responseCart = await client.GetAsync("api/Buyers/Cart");
                    if (responseCart.StatusCode == HttpStatusCode.OK)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            SetupCommonData();
                            //HttpContext.Session.SetString("CheckOut", "Done");
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
                    else if (responseCart.StatusCode == HttpStatusCode.NoContent)
                    {
                        SetupCommonData();
                        ViewBag.CheckOut = "Please add bouquet to cart";
                        return View("CheckOut");
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                // Handle any exception that occurs during the API call
                ModelState.AddModelError(string.Empty, "API Error: " + ex.Message);
            }

            // If there is an error, reload the SelectList and return the view with validation errors
            var occasions = await _service.GetSelectList<Occasion>("api/Occasions", "Id", "Type");
            var message = await _service.GetSelectList<Occasion>("api/Buyer", "Id", "Message");

            // Populate ViewBag with the SelectList data
            ViewBag.OccasionId = occasions;
            ViewBag.Message = message;
            return RedirectToAction("CheckOut");
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
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
                    SetupCommonData();
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
                    HttpContext.Response.Cookies.Append("AccessToken", content, cookieOptions);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(content);
                    ViewData["FullName"] = user.Email;
                    // Assuming you have the 'jwt' variable containing the JWT token string

                    // Extract the email claim
                    var userRole = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
                    if (userRole == "Admin")
                    {
                        return RedirectToAction("DashBoard", "Home");

                    }
                    if (userRole == "User")
                    {
                        return RedirectToAction("Index", "Customers");
                    }
                    if (userRole == "Florist")
                    {
                        return RedirectToAction("IndexOrder", "FloristAlls");
                    }
                    return RedirectToAction("Login", "Customers");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // If the API request fails, handle the error or display a message to the user
                    // For example, you can read the error response from the API and add an error message to the ModelState
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, "API Error: " + errorContent);
                    return View("~/Views/Customers/Forbidden.cshtml");
                }
                return View("~/Views/Customers/Invalid.cshtml");
            }
            catch (HttpRequestException ex)
            {
                // Handle any exception that occurs during the API call
                ModelState.AddModelError(string.Empty, "API Error: " + ex.Message);
            }
            return View();
        }
        public IActionResult Logout()
        {
            // Xóa token từ cookie
            HttpContext.Response.Cookies.Delete("AccessToken");

            // Chuyển hướng đến trang đăng nhập hoặc trang khác tùy chọn
            return RedirectToAction("Login", "Customers"); // Thay thế bằng đường dẫn trang đăng nhập của bạn
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /*[HttpGet]
        public async Task<IActionResult> RegisterFlorist()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            string token = HttpContext.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Customers"); // Redirect to the login page
            }
            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            SetupCommonData();


            // Decode the token to get the email
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            // Assuming you have the 'jwt' variable containing the JWT token string

            // Extract the email claim
            var userEmail = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;



            if (!string.IsNullOrEmpty(userEmail))
            {
                var responseUser = await client.GetAsync($"api/Auth/{userEmail}");

                SetupCommonData();
                var contentUser = await responseUser.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(contentUser);

                // Process the user's email
                var florist = new Florist();
                florist.Email = user.Email;
                florist.UserId = user.Id;
                var mess = TempData["Florist"] as string;
                ViewBag.Florist = mess;
                return View(florist);
            }
            return RedirectToAction("Login", "Customers");
        
    }
        */
        [HttpPost]
        public async Task<IActionResult> Register(UserRegister user)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Serialize the Receiver object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                // Send the POST request to the API
                var response = await client.PostAsync("api/Auth/signUp", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    SetupCommonData();
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
            return View(user);
        }

        /*[HttpPost]
        public async Task<IActionResult> RegisterFlorist(Florist florist, IFormFile? file)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");
                if (file == null)
                {
                    TempData["Florist"] = "Choose Your Logo";
                    return RedirectToAction("RegisterFlorist");
                }
                string fileName = Path.GetFileName(file.FileName);
                string file_path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/images/bouquet/", fileName);
                using (var stream = new FileStream(file_path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                florist.Logo = "/images/florist/" + fileName;

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
                    TempData["Florist"] = "Resgister Success Please Check your Mail";
                    return RedirectToAction("RegisterFlorist");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    TempData["Florist"] = "Your account already is florist";
                    return RedirectToAction("RegisterFlorist");
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
            return View(florist);
        }*/

        public async Task<IActionResult> ProductDetail(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var response = await client.GetAsync("api/Buyers/Product/" + id);

            if (response.IsSuccessStatusCode)
            {
                SetupCommonData();
                var content = await response.Content.ReadAsStringAsync();
                var bouquet = JsonConvert.DeserializeObject<Bouquet>(content);
                return View(bouquet);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            string token = HttpContext.Request.Cookies["PasswordResetToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("ForgotPassword", "Customers"); // Redirect to the login page
            }
            var email = TempData["Email"] as string;
            var mess = TempData["Mess"] as string;

            ViewBag.Email = email;
            ViewBag.Mess = mess;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");

            string token = HttpContext.Request.Cookies["PasswordResetToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Invalid", "Customers"); // Redirect to the login page
            }
            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var email = TempData["Email"] as string;
            ViewBag.Email = email;
            request.Token = token;
            // Serialize the post object to JSON
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Send the POST request to the API
            var response = await client.PostAsync("api/Auth/reset-password", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                SetupCommonData();
                return RedirectToAction("Login", "Customers");
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                SetupCommonData();
                TempData["Mess"] = "Token out of date";
                return RedirectToAction("ResetPassword", "Customers");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                SetupCommonData();
                TempData["Mess"] = "Password and Confirm Password must me the same";
                return RedirectToAction("ResetPassword", "Customers");
            }
            else
            {
                // Handle other error status codes
                // For example, you can log the error or display a generic error message.
                return Problem("Unable to rest users from the API.");
            }
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            var mess = TempData["Mess"] as string;
            ViewBag.Mess = mess;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                // Serialize the post object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");

                // Send the POST request to the API
                var response = await client.PostAsync($"api/Auth/forgot-password?email={email}", null);

                if (response.IsSuccessStatusCode)
                {
                    SetupCommonData();
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
                    TempData["Email"] = email;
                    // Lưu cookie vào response
                    HttpContext.Response.Cookies.Append("PasswordResetToken", content, cookieOptions);
                    return RedirectToAction("ResetPassword", "Customers");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // If the API request fails, handle the error or display a message to the user
                    // For example, you can read the error response from the API and add an error message to the ModelState
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, "API Error: " + errorContent);
                    TempData["Mess"] = "User not found";
                    return RedirectToAction("ForgotPassword", "Customers");
                }
                return RedirectToAction("ForgotPassword", "Customers");
            }
            catch (HttpRequestException ex)
            {
                // Handle any exception that occurs during the API call
                ModelState.AddModelError(string.Empty, "API Error: " + ex.Message);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> MyAccount()
        {
            var client = _httpClientFactory.CreateClient("MyApiClient");
            var mess = TempData["Mess"] as string;
            ViewBag.Mess = mess;
            string token = HttpContext.Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Customers");
            }
            if (!string.IsNullOrEmpty(token))
            {
                // Thêm token vào tiêu đề "Authorization"
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Decode the token to get the email
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            // Assuming you have the 'jwt' variable containing the JWT token string

            // Extract the email claim
            var userEmail = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
            var responseUser = await client.GetAsync($"api/Auth/{userEmail}");
            // Process the user's email
            if (responseUser.IsSuccessStatusCode)
            {
                SetupCommonData();
                var contentUser = await responseUser.Content.ReadAsStringAsync();
                var userContent = JsonConvert.DeserializeObject<User>(contentUser);

                var responseUserById = await client.GetAsync($"api/Users/{userContent.Id}");
                var contentUserById = await responseUserById.Content.ReadAsStringAsync();
                var userContentById = JsonConvert.DeserializeObject<User>(contentUserById);
                /*ViewBag.Firstname = userContent.Firstname;
                ViewBag.LastName = userContent.LastName;
                ViewBag.Email = userContent.Email;
                ViewBag.Phone = userContent.Phone;
                ViewBag.Address = userContent.Address;
                ViewBag.Dob = userContent.Dob;*/
                return View("MyAccount", userContentById);
            }
            else
            {
                // Handle other error status codes
                // For example, you can log the error or display a generic error message.
                return Problem("Unable to fetch users from the API.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccount(int id, [Bind("Id,HashPassword,Email,Phone,Address,Firstname,LastName,FullName,Gender,Dob,Role,StatusId")] User user)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("MyApiClient");

                string token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Customers");
                }
                if (!string.IsNullOrEmpty(token))
                {
                    // Thêm token vào tiêu đề "Authorization"
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Decode the token to get the email
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                // Assuming you have the 'jwt' variable containing the JWT token string

                // Extract the email claim
                var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;

                SetupCommonData();
                var responseUser = await client.GetAsync($"api/Auth/{email}");

                var contentUser = await responseUser.Content.ReadAsStringAsync();
                var userContent = JsonConvert.DeserializeObject<User>(contentUser);

                user.Id = userContent.Id;
                user.PasswordHash = userContent.PasswordHash;
                user.PasswordSalt = userContent.PasswordSalt;
                user.FullName = user.Firstname + " " + user.LastName;
                user.Role = userContent.Role;
                user.RefreshToken = userContent.RefreshToken;
                user.PasswordResetToken = userContent.PasswordResetToken;
                user.TokenCreated = userContent.TokenCreated;
                user.TokenExpires = userContent.TokenExpires;
                user.StatusId = userContent.StatusId;
                // Serialize the category object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                // Send the PUT request to the API
                /*var response = await client.PutAsync($"api/Users/{user.Id}", jsonContent);
                var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")*/
                ;

                // Send the PUT request to the API
                var response = await client.PutAsync($"api/Users/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("MyAccount");
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
            return RedirectToAction("MyAccount");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string password, string confirmPassword)
        {
            try
            {
                if (password != confirmPassword)
                {
                    TempData["Mess"] = "Password and Confirm Password not the same";
                    return RedirectToAction("MyAccount");
                }
                var client = _httpClientFactory.CreateClient("MyApiClient");

                string token = HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Customers");
                }
                if (!string.IsNullOrEmpty(token))
                {
                    // Thêm token vào tiêu đề "Authorization"
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // Decode the token to get the email
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                // Assuming you have the 'jwt' variable containing the JWT token string

                // Extract the email claim
                var userEmail = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
                SetupCommonData();
                // Send the PUT request to the API
                var response = await client.PutAsync($"api/Auth/change-password/{userEmail}?password={password}&confirmPassword={confirmPassword}", null);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Mess"] = "Success Change Password";
                    return RedirectToAction("MyAccount");
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
            return RedirectToAction("MyAccount");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Forbidden()
        {
            return View();
        }
        public IActionResult Invalid()
        {
            return View();
        }

        public IActionResult Bonlebon()
        {
            return View();
        }


        public IActionResult ShowToast(string mess)
        {
            return View(mess);
        }
    }
}
