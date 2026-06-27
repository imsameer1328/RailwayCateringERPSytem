using FrontRailwayCateringERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FrontRailwayCateringERP.Controllers
{
    [Authorize(Roles = "Super Admin, Manager")]
    public class JourneyController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7274/api/Journey";
        private readonly string _trainUrl = "https://localhost:7274/api/Train";
        private readonly string _userUrl = "https://localhost:7274/api/User";

        public JourneyController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> GetJourney()
        {
            ViewData["Title"] = "Journeys Management";
            ViewData["ActivePage"] = "Journeys";

            var response = await _httpClient.GetAsync($"{_apiUrl}/all");
            var json = await response.Content.ReadAsStringAsync();

            var journeys = JsonSerializer.Deserialize<List<JourneyViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (journeys == null)
            {
                journeys = new List<JourneyViewModel>();
            }

            return View(journeys);
        }

        public async Task<IActionResult> CreateJourney()
        {
            ViewData["Title"] = "Add New Journey";
            ViewData["ActivePage"] = "Journeys";

            var model = new JourneyViewModel();
            model.TrainList = await GetTrains();
            model.UserList = await GetUsers();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJourney(JourneyViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_apiUrl, content);
            return RedirectToAction("GetJourney");
        }

        public async Task<IActionResult> EditJourney(Guid id)
        {
            ViewData["Title"] = "Edit Journey";
            ViewData["ActivePage"] = "Journeys";

            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            var json = await response.Content.ReadAsStringAsync();

            var journey = JsonSerializer.Deserialize<JourneyViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (journey != null)
            {
                journey.TrainList = await GetTrains();
                journey.UserList = await GetUsers();
            }

            return View(journey);
        }

        [HttpPost]
        public async Task<IActionResult> EditJourney(Guid id, JourneyViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_apiUrl}/{id}", content);
            return RedirectToAction("GetJourney");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteJourney(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
            }

            return RedirectToAction("GetJourney");
        }

        private async Task<List<TrainViewModel>> GetTrains()
        {
            var response = await _httpClient.GetAsync($"{_trainUrl}/all");
            var json = await response.Content.ReadAsStringAsync();
            var trains = JsonSerializer.Deserialize<List<TrainViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (trains == null)
            {
                return new List<TrainViewModel>();
            }
            return trains;
        }

        private async Task<List<UserViewModel>> GetUsers()
        {
            var response = await _httpClient.GetAsync($"{_userUrl}/all");
            var json = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<UserViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (users == null)
            {
                return new List<UserViewModel>();
            }
            return users;
        }
    }
}
