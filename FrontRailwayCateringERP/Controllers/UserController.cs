using FrontRailwayCateringERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FrontRailwayCateringERP.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7274/api/User";
        private readonly string _roleUrl = "https://localhost:7274/api/Role";

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // GET — list all users
        public async Task<IActionResult> GetUser()
        {
            ViewData["Title"] = "Users Management";
            ViewData["ActivePage"] = "Users";

            var response = await _httpClient.GetAsync($"{_apiUrl}/all");
            var json = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<UserViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (users == null)
            {
                users = new List<UserViewModel>();
            }

            return View(users);   // ← automatically finds GetUser.cshtml
        }

        // GET — show create form
        public async Task<IActionResult> CreateUser()
        {
            ViewData["Title"] = "Add New User";
            ViewData["ActivePage"] = "Users";

            var model = new UserViewModel();
            model.RoleList = await GetRoles();    // ← fill the list
            return View(model);
        }

        // POST — save new user
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_apiUrl, content);
            return RedirectToAction("GetUser");
        }

        // GET — show edit form
        public async Task<IActionResult> EditUser(Guid id)
        {
            ViewData["Title"] = "Edit User";
            ViewData["ActivePage"] = "Users";

            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            user.RoleList = await GetRoles();
            return View(user);    // ← automatically finds EditUser.cshtml
        }

        // POST — save edited user
        [HttpPost]
        public async Task<IActionResult> EditUser(Guid id, UserViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_apiUrl}/{id}", content);
            return RedirectToAction("GetUser");
        }

        // POST — delete user
        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
            }
            return RedirectToAction("GetUser");
        }

        // ── Private helper — just fetches roles from API ──
        private async Task<List<RoleViewModel>> GetRoles()
        {
            var response = await _httpClient.GetAsync($"{_roleUrl}/all");
            var json = await response.Content.ReadAsStringAsync();
            var roles = JsonSerializer.Deserialize<List<RoleViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (roles == null)
            {
                return new List<RoleViewModel>();
            }
            return roles;
        }
    }
}