using FrontRailwayCateringERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace FrontRailwayCateringERP.Controllers
{
    [Authorize(Roles = "Waiter, Super Admin, Manager")]
    public class WaiterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7274/api";

        public WaiterController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> MyOrders()
        {
            ViewData["Title"] = "My Orders";
            ViewData["ActivePage"] = "MyOrders";

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _httpClient.GetAsync($"{_apiUrl}/Order/by-user/{userId}");
            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<OrderViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<OrderViewModel>();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> PickupItem(Guid itemId)
        {
            var payload = new { kitchenStatus = "PickedUp" };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiUrl}/OrderItem/{itemId}", content);

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Failed to pick up item";

            return RedirectToAction("MyOrders");
        }

        [HttpPost]
        public async Task<IActionResult> DeliverItem(Guid itemId, Guid orderId)
        {
            var payload = new { kitchenStatus = "Delivered" };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_apiUrl}/OrderItem/{itemId}", content);

            var orderResponse = await _httpClient.GetAsync($"{_apiUrl}/Order/{orderId}");
            if (orderResponse.IsSuccessStatusCode)
            {
                var orderJson = await orderResponse.Content.ReadAsStringAsync();
                var order = JsonSerializer.Deserialize<OrderViewModel>(orderJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (order != null && order.OrderItems.All(i => i.KitchenStatus == "Delivered"))
                {
                    var updatePayload = new
                    {
                        coachNumber = order.CoachNumber ?? "",
                        seatNumber = order.SeatNumber ?? "",
                        status = "Completed",
                        totalAmount = order.TotalAmount,
                        userId = order.UserId.ToString(),
                        journeyId = order.JourneyId.ToString()
                    };
                    var updateContent = new StringContent(JsonSerializer.Serialize(updatePayload), Encoding.UTF8, "application/json");
                    await _httpClient.PutAsync($"{_apiUrl}/Order/{orderId}", updateContent);
                }
            }

            return RedirectToAction("MyOrders");
        }
    }
}
