using FrontRailwayCateringERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FrontRailwayCateringERP.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class RoleController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7274/api/Role";

        public RoleController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // GET — list all roles
        public async Task<IActionResult> GetRole()
        {
            ViewData["Title"] = "Roles Management";
            ViewData["ActivePage"] = "Roles";

            var response = await _httpClient.GetAsync($"{_apiUrl}/all");
            var json = await response.Content.ReadAsStringAsync();

            var roles = JsonSerializer.Deserialize<List<RoleViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (roles == null)
            {
                roles = new List<RoleViewModel>();
            }

            return View(roles);   // ← automatically finds GetRole.cshtml
        }

        // GET — show create form
        public IActionResult CreateRole()
        {
            ViewData["Title"] = "Add New Role";
            ViewData["ActivePage"] = "Roles";
            return View();        // ← automatically finds CreateRole.cshtml
        }

        // POST — save new role
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_apiUrl, content);
            return RedirectToAction("GetRole");
        }

        // GET — show edit form
        public async Task<IActionResult> EditRole(Guid id)
        {
            ViewData["Title"] = "Edit Role";
            ViewData["ActivePage"] = "Roles";

            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            var json = await response.Content.ReadAsStringAsync();

            var role = JsonSerializer.Deserialize<RoleViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(role);    // ← automatically finds EditRole.cshtml
        }

        // POST — save edited role
        [HttpPost]
        public async Task<IActionResult> EditRole(Guid id, RoleViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_apiUrl}/{id}", content);
            return RedirectToAction("GetRole");
        }

        // POST — delete role
        [HttpPost]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("GetRole");
        }
    }
}