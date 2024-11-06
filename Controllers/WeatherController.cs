using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq; // Добавляем директиву using
using WebApplication261024.Models;

namespace WebApplication261024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")] // Указываем тип возвращаемых данных
    public class WeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public WeatherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("GetWeather")]
        public async Task<IActionResult> GetWeather(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest(new { Error = "Please enter a city name." });
            }

            string apiKey = "46ffaf6a42384bb507cde5a9aab789ca"; // Ваш API ключ
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var weatherData = JObject.Parse(responseBody);

                var result = new WeatherResultDTO
                {
                    City = city,
                    Temperature = (float)weatherData["main"]["temp"],
                    Description = (string)weatherData["weather"][0]["description"]
                };

                return Ok(result);
            }
            catch (HttpRequestException)
            {
                return StatusCode(500, new { Error = "Unable to get weather data. Please try again later." });
            }
        }
    }
}


