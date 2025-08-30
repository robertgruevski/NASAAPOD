using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NASAAPOD.Models;

namespace NASAAPOD.Controllers
{
	public class HomeController : Controller
	{
		private const string NASA_API_KEY = "DEMO_KEY"; // Replace with your actual API key
		private const string NASA_API_URL = "https://api.nasa.gov/planetary/apod";
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			var apodData = new Apod();

			try
			{
				var httpClient = _httpClientFactory.CreateClient();
				var response = await httpClient.GetAsync($"{NASA_API_URL}?api_key={NASA_API_KEY}");

				if (response.IsSuccessStatusCode)
				{
					var jsonResponseString = await response.Content.ReadAsStringAsync();
					apodData = JsonSerializer.Deserialize<Apod>(jsonResponseString, new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true
					});
				}
				else
				{
					ViewBag.ErrorMessage = "Failed to retrieve data from NASA API.";
				}
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = $"An error occurred while fetching data. {ex.Message}";
			}

			return View(apodData);
		}
	}
}
