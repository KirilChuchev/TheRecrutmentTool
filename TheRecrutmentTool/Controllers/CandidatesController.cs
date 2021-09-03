namespace TheRecrutmentTool.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using TheRecrutmentTool.Helpers;
    using System.Collections.Generic;
    using TheRecrutmentTool.Services;
    using Microsoft.Extensions.Logging;
    using TheRecrutmentTool.Data.Models;
    using TheRecrutmentTool.ViewModels.Response;
    using TheRecrutmentTool.ViewModels.Candidate;

    [ApiController]
    [Route("[controller]")]
    public class CandidatesController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger; // TODO To record each action related to creating a new candidate. 
        private readonly IRecruitersServices recruitersServices;
        private readonly ICandidatesServices candidatesServices;
        private readonly ISkillsServices skillsServices;

        public CandidatesController(
            ILogger<WeatherForecastController> logger, 
            IRecruitersServices recruitersServices,
            ICandidatesServices candidatesServices,
            ISkillsServices skillsServices)
        {
            _logger = logger;
            this.recruitersServices = recruitersServices;
            this.candidatesServices = candidatesServices;
            this.skillsServices = skillsServices;
        }

        [HttpGet, ActionName("/")]
        //[Route("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] int id)
        {
            var candidate = await this.candidatesServices.GetByIdAsync(id);

            if (candidate == null)
            {
                var message = "This candidate do not exists.";
                return StatusCode(
                StatusCodes.Status400BadRequest,
                new Response { Status = "Error", Message = message });
            };

            var model = new CandidateViewModel()
            {
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                BirthDate = candidate.BirthDate.ToString(),
                Bio = candidate.Bio,
                Email = candidate.Email,
            };

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CandidateCreateModel model)
        {
            try
            {
                var isCandidateExist = await this.candidatesServices.IsCandidateExistsAsync(model.Email);

                if (isCandidateExist)
                {
                    var message = "This candidate already exists.";
                    return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = message });
                }

                var recruiter = new Recruiter()
                {
                    LastName = model.Recruiter.LastName,
                    Email = model.Recruiter.Email,
                    Country = model.Recruiter.Country,
                };

                var recruiterId = await this.recruitersServices.CreateIfNotExistsAsync(recruiter);

                var skillModels = model.Skills.ToArray();
                var skills = new List<Skill>();

                foreach (var skillModel in skillModels)
                {
                    var skillId = await this.skillsServices.CreateIfNotExistsAsync(skillModel.Name);
                    var skill = await this.skillsServices.GetSkillByIdAsync(skillId);
                    skills.Add(skill);
                }

                var newCandidate = new Candidate()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Bio = model.Bio,
                    BirthDate = DateTime.Parse(model.BirthDate),
                    RecruiterId = recruiterId,
                    Skills = skills,
                };

                await this.candidatesServices.CreateAsync(newCandidate);

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
