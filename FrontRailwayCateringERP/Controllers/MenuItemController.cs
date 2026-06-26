using FrontRailwayCateringERP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FrontRailwayCateringERP.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7274/api/MenuItem";

        public MenuItemController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> GetMenuItem()
        {
            ViewData["Title"] = "Menu Items Management";
            ViewData["ActivePage"] = "MenuItems";

            var response = await _httpClient.GetAsync($"{_apiUrl}/all");
            var json = await response.Content.ReadAsStringAsync();

            var menuItems = JsonSerializer.Deserialize<List<MenuItemViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (menuItems == null)
            {
                menuItems = new List<MenuItemViewModel>();
            }

            return View(menuItems);
        }

        public IActionResult CreateMenuItem()
        {
            ViewData["Title"] = "Add New Menu Item";
            ViewData["ActivePage"] = "MenuItems";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuItem(MenuItemViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_apiUrl, content);
            return RedirectToAction("GetMenuItem");
        }

        public async Task<IActionResult> EditMenuItem(Guid id)
        {
            ViewData["Title"] = "Edit Menu Item";
            ViewData["ActivePage"] = "MenuItems";

            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            var json = await response.Content.ReadAsStringAsync();

            var menuItem = JsonSerializer.Deserialize<MenuItemViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(menuItem);
        }

        [HttpPost]
        public async Task<IActionResult> EditMenuItem(Guid id, MenuItemViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_apiUrl}/{id}", content);
            return RedirectToAction("GetMenuItem");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMenuItem(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("GetMenuItem");
        }
    }
}
