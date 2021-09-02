namespace TheRecrutmentTool.Services
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TheRecrutmentTool.Data;
    using TheRecrutmentTool.Data.Models;
    using TheRecrutmentTool.ViewModels.Candidate;

    public class CandidatesServices : ICandidatesServices
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IRecruitersServices recruitersServices;
        private readonly ISkillsServices skillsServices;

        public CandidatesServices(
            ApplicationDbContext dbContext, 
            IRecruitersServices recruitersServices,
            ISkillsServices skillsServices)
        {
            this.dbContext = dbContext;
            this.recruitersServices = recruitersServices;
            this.skillsServices = skillsServices;
        }

        public async Task<bool> IsCandidateExists(string email)
        {
            return (await this.dbContext.Candidates.FirstOrDefaultAsync(x => x.Email == email)) != null;
        }

        public async Task<int> Create(CandidateCreateModel model)
        {
            var newCandidate = new Candidate()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                BirthDate = DateTime.Parse(model.BirthDate),
                Bio = model.Bio,
            };

            // Recruiter
            var isRecruiterExists = await this.recruitersServices.IsRecruiterExists(model.Recruiter.Email);
            if (isRecruiterExists)
            {
                newCandidate.RecruiterId = await this.recruitersServices.GetRecruiterId(model.Recruiter.Email);
                var recruiter = await this.dbContext.Recruiters.FirstOrDefaultAsync(x => x.Email == model.Recruiter.Email);
                recruiter.ExperienceLevel += 1;
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                var recruiterId = await this.recruitersServices.Create(model.Recruiter);
                newCandidate.RecruiterId = recruiterId;
            }

            //Skills
            var dbSkills = await this.dbContext.Skills.ToListAsync();
            var newSkillModels = model.Skills.Where(x => dbSkills.Any(y => x.Name == y.Name)).ToArray();



            var newSkills = newSkillModels.Select(x => new Skill()
            {
                Name = x.Name,
            }).ToArray();

            await this.dbContext.Skills.AddRangeAsync(newSkills);
            await this.dbContext.SaveChangesAsync();
            //newCandidate.Skills = await this.dbContext.Skills.Where(x => model.Skills.Any(y => y.Name == x.Name)).ToArrayAsync();

            newCandidate.Skills = newSkills;

            // Create Candidate
            var entity = await this.dbContext.Candidates.AddAsync(newCandidate);
            await this.dbContext.SaveChangesAsync();

            return entity.Entity.Id;
        }
    }
}
