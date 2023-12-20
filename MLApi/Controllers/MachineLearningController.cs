using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
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
        [HttpPut(Name = "UpdateContext")]
        public async Task<ActionResult> UpdateContext()
        {
            _logger.LogInformation("UpdateContext called");

            HousePriceTraining.Train(@"C:\Users\M89501426\source\repos\MLApi\MLApi\MLSetting\HousePrice.mlnet");

            return await Task.FromResult(NoContent());
        }

        //[HttpPost(Name = "Predict")]
        //public async Task<ActionResult> Predict(ModelInput input)
        //{
        //    _logger.LogInformation("Predict called");

        //    var predictionHandler = async (PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool, ModelInput input) => await Task.FromResult(predictionEnginePool.Predict(input));

        //    //predictionHandler();

        //    this.Predict(input);

        //    return NoContent();
        //}
    }
}