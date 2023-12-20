using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using MLApi.MLSetting;
using MLApi.Models;
using System.Net;

namespace MLApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MachineLearningController(ILogger<MachineLearningController> _logger) : ControllerBase
    {
        //private readonly ILogger<MachineLearningController> _logger;

        //public MachineLearningController(ILogger<MachineLearningController> logger)
        //{
        //    _logger = logger;
        //}

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpPut(Name = "UpdateContext")]
        public ActionResult UpdateContext()
        {
            _logger.LogInformation("UpdateContext called");

            HousePriceTraining.Train(@"C:\Users\M89501426\source\repos\MLApi\MLApi\MLSetting\HousePrice.mlnet");

            return NoContent();
        }
    }
}