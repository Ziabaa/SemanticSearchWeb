using Microsoft.AspNetCore.Mvc;
using SemanticSearch.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace SemanticSearch.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _pythonApiUrl = "http://127.0.0.1:8080/program/execute";

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new SearchViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SearchViewModel model)
        {
            if (string.IsNullOrEmpty(model.Query))
            {
                return View(model);
            }

            try
            {
                var request = new { query = model.Query };
                var response = await _httpClient.PostAsJsonAsync(_pythonApiUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<SemanticSearchApiResponse>(responseBody);

                    model.AiResponse = apiResponse?.Answer ?? "Пусто";
                    model.IsSearched = true;

                    if (apiResponse?.FoundFunctions != null && apiResponse.FoundFunctions.Count > 0)
                    {
                        model.ExecutedFunctions = apiResponse.FoundFunctions
                            .Select(f => new ExecutedFunction
                            {
                                Name = f.Name,
                                Params = f.Params ?? new Dictionary<string, object>()
                            })
                            .ToList();
                    }
                }
                else
                {
                    model.AiResponse = $"Помилка API: {response.StatusCode}";
                    model.IsSearched = true;
                }
            }
            catch (HttpRequestException ex)
            {
                model.AiResponse = $"Не вдалося підключитися до сервера: {ex.Message}";
                model.IsSearched = true;
            }
            catch (JsonException ex)
            {
                model.AiResponse = $"Помилка парсингу JSON: {ex.Message}";
                model.IsSearched = true;
            }
            catch (Exception ex)
            {
                model.AiResponse = $"Помилка: {ex.Message}";
                model.IsSearched = true;
            }

            return View(model);
        }
    }

}