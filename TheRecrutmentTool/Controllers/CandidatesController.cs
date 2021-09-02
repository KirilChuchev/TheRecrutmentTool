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
    using System.Linq;

    [ApiController]
    [Route("[controller]")]
    public class CandidatesController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
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
                var isCandidateExist = await this.candidatesServices.IsCandidateExistsAsync(model.Email);

                if (isCandidateExist)
                {
                    var message = "This candidate already exists.";
                    return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = message });
                }

                //              {
                //                  "firstName": "Ingaborg",
                //"lastName": "Sothern",
                //"email": "isothern0@usatoday.com",
                //"bio": "Long and descriptive biography of the candidate",
                //"birthDate": "1999-05-22",
                //"skills": [

                //        { "name": "VueJS"},
                //  { "name": "Java" },
                //  { "name": "Angular"},
                //  { "name": "C#" }
                //],
                //"recruiter": {
                //                      "lastName": "Frye",
                //	"email": "pfrye1@whitehouse.gov",
                //	"country": "Russia"

                //      }
                //              }

                //// Recruiter
                //var isRecruiterExists = await this.recruitersServices.IsRecruiterExists(model.Recruiter.Email);
                //if (isRecruiterExists)
                //{
                //    newCandidate.RecruiterId = await this.recruitersServices.GetRecruiterId(model.Recruiter.Email);
                //    var recruiter = await this.dbContext.Recruiters.FirstOrDefaultAsync(x => x.Email == model.Recruiter.Email);
                //    recruiter.ExperienceLevel += 1;
                //    await this.dbContext.SaveChangesAsync();
                //}
                //else
                //{
                //    var recruiterId = await this.recruitersServices.Create(model.Recruiter);
                //    newCandidate.RecruiterId = recruiterId;
                //}

                var recruiter = new Recruiter()
                {
                    LastName = model.Recruiter.LastName,
                    Email = model.Recruiter.Email,
                    Country = model.Recruiter.Country,
                };

                var recruiterId = await this.recruitersServices.CreateIfNotExistsAsync(recruiter);

                var skillModels = model.Skills.ToArray();
                var skills = new List<Skill>();

                //.Select(async y => await this.skillsServices.GetSkillByIdAsync(y));

                foreach (var skillModel in skillModels)
                {
                    var skillId = await this.skillsServices.CreateIfNotExistsAsync(skillModel.Name);
                    var skill = await this.skillsServices.GetSkillByIdAsync(skillId);
                    //skill.Candidates.Add(newCandidate)
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
