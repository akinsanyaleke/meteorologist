using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace weatherforecast.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;


        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string province)
        {
            Province prov;
            if (!Enum.TryParse(province, out prov))
            {
                return BadRequest();
            }

            var uri = new Uri("https://weatherforecast-csharp.tap.aks.lekeakinsanya.com/WeatherForecast/");

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = uri;

            var response = await httpClient.GetAsync($"PROVINCES/{province}");
            if (response.IsSuccessStatusCode)
            {
                var weather = await response.Content.ReadFromJsonAsync<WeatherForecast>();
                if (weather == null)
                    return NotFound();
                weather.Summary = $"The weather in {province} is {weather.TemperatureC.ToString()}";
                Console.WriteLine(weather);
                return Ok(weather);
            }

            Console.WriteLine("Internal server Error");
            return Problem();

        }
    }
    public enum Province
    {
        ON,
        AB,
        VA
    }
}


