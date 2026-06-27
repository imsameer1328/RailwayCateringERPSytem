using FrontRailwayCateringERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FrontRailwayCateringERP.Controllers
{
    [Authorize(Roles = "Super Admin, Manager")]
    public class IngredientController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7274/api/Ingredient";

        public IngredientController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> GetIngredient()
        {
            ViewData["Title"] = "Ingredients Management";
            ViewData["ActivePage"] = "Ingredients";

            var response = await _httpClient.GetAsync($"{_apiUrl}/all");
            var json = await response.Content.ReadAsStringAsync();

            var ingredients = JsonSerializer.Deserialize<List<IngredientViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(ingredients ?? new List<IngredientViewModel>());
        }

        public IActionResult CreateIngredient()
        {
            ViewData["Title"] = "Add New Ingredient";
            ViewData["ActivePage"] = "Ingredients";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredient(IngredientViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_apiUrl, content);
            return RedirectToAction("GetIngredient");
        }

        public async Task<IActionResult> EditIngredient(Guid id)
        {
            ViewData["Title"] = "Edit Ingredient";
            ViewData["ActivePage"] = "Ingredients";

            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            var json = await response.Content.ReadAsStringAsync();

            var ingredient = JsonSerializer.Deserialize<IngredientViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(ingredient);
        }

        [HttpPost]
        public async Task<IActionResult> EditIngredient(Guid id, IngredientViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_apiUrl}/{id}", content);
            return RedirectToAction("GetIngredient");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIngredient(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("GetIngredient");
        }
    }
}
