using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MR.GoldRedis.Attributes;
using MR.GoldRedis.Services;

namespace MR.GoldRedis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IResponseCacheService _responseCacheService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IResponseCacheService responseCacheService)
        {
            _logger = logger;
            _responseCacheService = responseCacheService;
        }

        [HttpGet("getAll")]
        [Cache(1000)]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherAsync(string keysearch, int pageSize, int pageIndex)
        {
            var result = new List<WeatherForecast>()
            {
                new WeatherForecast()
                {
                    Name = "Binh"
                },
                new WeatherForecast()
                {
                    Name = "An"
                },
                new WeatherForecast()
                {
                    Name = "Hoa"
                },
                new WeatherForecast()
                {
                    Name = "Bay"
                }
            };
            return Ok(result);
        }
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            //var enpoint = HttpContext.Request.Path;
            await _responseCacheService.RemoveCacheResponse("/WeatherForecast/getAll");
            return Ok();
        }
    }
}
