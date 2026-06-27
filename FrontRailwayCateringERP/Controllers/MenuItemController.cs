using FrontRailwayCateringERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FrontRailwayCateringERP.Controllers
{
    [Authorize(Roles = "Super Admin, Manager")]
    public class MenuItemController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7274/api/MenuItem";
        private readonly string _ingredientApiUrl = "https://localhost:7274/api/Ingredient";

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
            }) ?? new List<MenuItemViewModel>();

            var allIngredients = await GetAllIngredients();
            var ingredientLookup = allIngredients.ToDictionary(i => i.IngredientId, i => i.Name);

            foreach (var item in menuItems)
            {
                if (item.Ingredients != null)
                {
                    foreach (var ing in item.Ingredients)
                    {
                        if (ingredientLookup.TryGetValue(ing.IngredientId, out var name))
                            ing.IngredientName = name;
                    }
                }
            }

            return View(menuItems);
        }

        public async Task<IActionResult> CreateMenuItem()
        {
            ViewData["Title"] = "Add New Menu Item";
            ViewData["ActivePage"] = "MenuItems";
            ViewBag.AllIngredients = await GetAllIngredients();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuItem(MenuItemViewModel model)
        {
            var payload = new
            {
                name = model.Name,
                category = model.Category,
                price = model.Price,
                availabilityStatus = model.AvailabilityStatus,
                menuItemIngredients = model.Ingredients?.Where(i => i.IngredientId != Guid.Empty).Select(i => new
                {
                    ingredientId = i.IngredientId,
                    quantityNeeded = i.QuantityNeeded,
                    unit = i.Unit ?? ""
                }).ToList()
            };

            var json = JsonSerializer.Serialize(payload);
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

            ViewBag.AllIngredients = await GetAllIngredients();

            return View(menuItem);
        }

        [HttpPost]
        public async Task<IActionResult> EditMenuItem(Guid id, MenuItemViewModel model)
        {
            var payload = new
            {
                name = model.Name,
                category = model.Category,
                price = model.Price,
                availabilityStatus = model.AvailabilityStatus,
                menuItemIngredients = model.Ingredients?.Where(i => i.IngredientId != Guid.Empty).Select(i => new
                {
                    ingredientId = i.IngredientId,
                    quantityNeeded = i.QuantityNeeded,
                    unit = i.Unit ?? ""
                }).ToList()
            };

            var json = JsonSerializer.Serialize(payload);
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

        private async Task<List<IngredientViewModel>> GetAllIngredients()
        {
            var response = await _httpClient.GetAsync($"{_ingredientApiUrl}/all");
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<IngredientViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<IngredientViewModel>();
        }
    }
}
