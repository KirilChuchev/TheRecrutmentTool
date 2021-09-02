namespace TheRecrutmentTool.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using TheRecrutmentTool.Helpers;
    using System.Collections.Generic;
    using TheRecrutmentTool.Services;
    using Microsoft.Extensions.Logging;
    using TheRecrutmentTool.Data.Models;
    using TheRecrutmentTool.ViewModels.Candidate;
    using TheRecrutmentTool.ViewModels.Response;

    [ApiController]
    [Route("[controller]")]
    public class CandidatesController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRecruitersServices recruitersServices;
        private readonly ICandidatesServices candidatesServices;

        public CandidatesController(
            ILogger<WeatherForecastController> logger, 
            IRecruitersServices recruitersServices,
            ICandidatesServices candidatesServices)
        {
            _logger = logger;
            this.recruitersServices = recruitersServices;
            this.candidatesServices = candidatesServices;
        }

        [HttpGet]
        public IEnumerable<Candidate> Get([FromHeader] string id)
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CandidateCreateModel model)
        {
            try
            {
                var isCandidateExist = await this.candidatesServices.IsCandidateExists(model.Email);

                if (isCandidateExist)
                {
                    var message = "This candidate already exists.";
                    return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = message });
                }

                await this.candidatesServices.Create(model);

                return Ok("Candidate sucessfully created.");
            }
            catch (Exception ex)
            {
                var message = ExceptionMessageCreator.CreateMessage(ex);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = message });
            }
        }

    }
}
