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

        public async Task<bool> IsCandidateExistsAsync(string email)
        {
            return (await this.dbContext.Candidates.FirstOrDefaultAsync(x => x.Email == email)) != null;
        }

        public async Task<int> CreateAsync(Candidate candidate)
        {

            var entity = await this.dbContext.Candidates.AddAsync(candidate);
            await this.dbContext.SaveChangesAsync();

            return entity.Entity.Id;
        }

        public async Task<Candidate> GetByIdAsync(int id)
        {
           return await this.dbContext.Candidates.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
