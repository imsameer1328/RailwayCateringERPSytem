using FrontRailwayCateringERP.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace FrontRailwayCateringERP.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Get");

            ViewBag.UsersExist = await CheckUsersExist();
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonSerializer.Serialize(new { username = model.Username, password = model.Password });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7274/api/User/login", content);
            if (!response.IsSuccessStatusCode)
            {
                model.ErrorMessage = "Invalid username or password";
                return View(model);
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var userData = JsonSerializer.Deserialize<JsonElement>(responseJson);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userData.GetProperty("username").GetString() ?? ""),
                new Claim(ClaimTypes.NameIdentifier, userData.GetProperty("userId").GetString() ?? ""),
                new Claim(ClaimTypes.GivenName, userData.GetProperty("fullName").GetString() ?? ""),
                new Claim(ClaimTypes.Role, userData.GetProperty("roleName").GetString() ?? "")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Get");
        }

        public async Task<IActionResult> Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Get");

            if (await CheckUsersExist())
                return RedirectToAction("Login");

            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await CheckUsersExist())
            {
                model.ErrorMessage = "Registration is closed. A Super Admin already exists.";
                return View(model);
            }

            var json = JsonSerializer.Serialize(new
            {
                fullName = model.FullName,
                username = model.Username,
                password = model.Password,
                phone = model.Phone ?? ""
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7274/api/User/register", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                try
                {
                    var errorData = JsonSerializer.Deserialize<JsonElement>(errorJson);
                    model.ErrorMessage = errorData.GetProperty("message").GetString() ?? "Registration failed";
                }
                catch
                {
                    model.ErrorMessage = "Registration failed. Please try again.";
                }
                return View(model);
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var userData = JsonSerializer.Deserialize<JsonElement>(responseJson);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userData.GetProperty("username").GetString() ?? ""),
                new Claim(ClaimTypes.NameIdentifier, userData.GetProperty("userId").GetString() ?? ""),
                new Claim(ClaimTypes.GivenName, userData.GetProperty("fullName").GetString() ?? ""),
                new Claim(ClaimTypes.Role, userData.GetProperty("roleName").GetString() ?? "")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Get");
        }

        [Authorize]
        public IActionResult Get()
        {
            ViewData["Title"] = "Dashboard";
            ViewData["ActivePage"] = "Dashboard";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private async Task<bool> CheckUsersExist()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7274/api/User/all");
                if (!response.IsSuccessStatusCode) return true;
                var json = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<JsonElement>(json);
                return users.GetArrayLength() > 0;
            }
            catch
            {
                return true;
            }
        }
    }
}
