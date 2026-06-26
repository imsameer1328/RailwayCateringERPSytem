using FrontRailwayCateringERP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FrontRailwayCateringERP.Controllers
{
    public class TrainController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7274/api/Train";

        public TrainController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // GET — list all trains
        public async Task<IActionResult> GetTrain()
        {
            ViewData["Title"] = "Train Management";
            ViewData["ActivePage"] = "Trains";

            var response = await _httpClient.GetAsync($"{_apiUrl}/all");
            var json = await response.Content.ReadAsStringAsync();

            var trains = JsonSerializer.Deserialize<List<TrainViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (trains == null)
            {
                trains = new List<TrainViewModel>();
            }

            return View(trains);   // ← automatically finds GetTrain.cshtml
        }

        // GET — show create form
        public IActionResult CreateTrain()
        {
            ViewData["Title"] = "Add New Train";
            ViewData["ActivePage"] = "Trains";
            return View();         // ← automatically finds CreateTrain.cshtml
        }

        // POST — save new train
        [HttpPost]
        public async Task<IActionResult> CreateTrain(TrainViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_apiUrl, content);
            return RedirectToAction("GetTrain");
        }

        // GET — show edit form
        public async Task<IActionResult> EditTrain(Guid id)
        {
            ViewData["Title"] = "Edit Train";
            ViewData["ActivePage"] = "Trains";

            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            var json = await response.Content.ReadAsStringAsync();

            var train = JsonSerializer.Deserialize<TrainViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(train);    // ← automatically finds EditTrain.cshtml
        }

        // POST — save edited train
        [HttpPost]
        public async Task<IActionResult> EditTrain(Guid id, TrainViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_apiUrl}/{id}", content);
            return RedirectToAction("GetTrain");
        }

        // POST — delete train
        [HttpPost]
        public async Task<IActionResult> DeleteTrain(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("GetTrain");
        }
    }
}
