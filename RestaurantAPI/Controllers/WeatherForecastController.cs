using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {        
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForcastService _weatherForcastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForcastService weatherForcastService)
        {
            _logger = logger;
            _weatherForcastService = weatherForcastService;
        }

        [HttpPost]
        [Route("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> Generate([FromQuery]int resultCounter, [FromBody]ValueTemperature temperature)
        {            
            if(resultCounter > 0 && temperature.MaxTemperature>temperature.MinTemperature)
            {
                var result = _weatherForcastService.Get(resultCounter, temperature.MinTemperature, temperature.MaxTemperature);
                return Ok(result);
            }
            return StatusCode(400, "Bad request");
        }
    }
}
