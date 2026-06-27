using FrontRailwayCateringERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace FrontRailwayCateringERP.Controllers
{
    [Authorize(Roles = "Super Admin, Manager, Waiter")]
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7274/api";

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> GetOrders()
        {
            ViewData["Title"] = "Orders Management";
            ViewData["ActivePage"] = "Orders";

            var response = await _httpClient.GetAsync($"{_apiUrl}/Order/all");
            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<OrderViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<OrderViewModel>();

            return View(orders.OrderByDescending(o => o.CreatedAt).ToList());
        }

        public async Task<IActionResult> CreateOrder()
        {
            ViewData["Title"] = "Create Order";
            ViewData["ActivePage"] = "Orders";

            var model = new CreateOrderViewModel();
            model.JourneyList = await GetJourneys();
            model.MenuItems = await GetMenuItems();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model, List<Guid> selectedItemIds, List<int> quantities)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            decimal totalAmount = 0;
            var itemDetails = new List<(Guid MenuItemId, int Qty, decimal Price)>();

            var allItems = await GetMenuItems();
            for (int i = 0; i < selectedItemIds.Count; i++)
            {
                var qty = i < quantities.Count ? quantities[i] : 0;
                if (qty <= 0) continue;
                var menuItem = allItems.FirstOrDefault(m => m.MenuItemId == selectedItemIds[i]);
                if (menuItem == null) continue;
                totalAmount += menuItem.Price * qty;
                itemDetails.Add((menuItem.MenuItemId, qty, menuItem.Price));
            }

            if (itemDetails.Count == 0)
            {
                model.JourneyList = await GetJourneys();
                model.MenuItems = allItems;
                ModelState.AddModelError("", "Please select at least one item with quantity > 0");
                return View(model);
            }

            var orderPayload = new
            {
                coachNumber = model.CoachNumber ?? "",
                seatNumber = model.SeatNumber ?? "",
                status = "Pending",
                totalAmount,
                userId,
                journeyId = model.JourneyId.ToString()
            };

            var orderJson = JsonSerializer.Serialize(orderPayload);
            var orderContent = new StringContent(orderJson, Encoding.UTF8, "application/json");
            var orderResponse = await _httpClient.PostAsync($"{_apiUrl}/Order", orderContent);
            if (!orderResponse.IsSuccessStatusCode)
            {
                model.JourneyList = await GetJourneys();
                model.MenuItems = allItems;
                ModelState.AddModelError("", "Failed to create order");
                return View(model);
            }

            var orderResponseJson = await orderResponse.Content.ReadAsStringAsync();
            var orderData = JsonSerializer.Deserialize<JsonElement>(orderResponseJson);
            var orderId = orderData.GetProperty("orderId").GetString();

            foreach (var (menuItemId, qty, price) in itemDetails)
            {
                var itemPayload = new
                {
                    quantity = qty,
                    unitPrice = price,
                    kitchenStatus = "Pending",
                    orderId,
                    menuItemId = menuItemId.ToString()
                };
                var itemJson = JsonSerializer.Serialize(itemPayload);
                var itemContent = new StringContent(itemJson, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync($"{_apiUrl}/OrderItem", itemContent);
            }

            return RedirectToAction("GetOrders");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/Order/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = await response.Content.ReadAsStringAsync();
            }
            return RedirectToAction("GetOrders");
        }

        private async Task<List<JourneyViewModel>> GetJourneys()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/Journey/all");
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<JourneyViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<JourneyViewModel>();
        }

        private async Task<List<MenuItemViewModel>> GetMenuItems()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/MenuItem/all");
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<MenuItemViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<MenuItemViewModel>();
        }
    }
}
