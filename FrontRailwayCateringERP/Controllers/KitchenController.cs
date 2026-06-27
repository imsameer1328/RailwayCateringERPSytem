using FrontRailwayCateringERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FrontRailwayCateringERP.Controllers
{
    [Authorize(Roles = "Super Admin, Manager, Kitchen Staff")]
    public class KitchenController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7274/api";

        public KitchenController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> GetOrders()
        {
            ViewData["Title"] = "Kitchen Orders";
            ViewData["ActivePage"] = "Kitchen";

            var response = await _httpClient.GetAsync($"{_apiUrl}/Order/all");
            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<OrderViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<OrderViewModel>();

            orders = orders.Where(o => o.OrderItems.Any(i =>
                i.KitchenStatus == "Pending" ||
                i.KitchenStatus == "Cooking" ||
                i.KitchenStatus == "Ready"))
                           .OrderBy(o => o.CreatedAt)
                           .ToList();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItemStatus(Guid itemId, string status)
        {
            var payload = new { kitchenStatus = status };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiUrl}/OrderItem/{itemId}", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to update item status";
            }

            return RedirectToAction("GetOrders");
        }
    }
}
