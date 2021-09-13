namespace TheRecrutmentTool.Services
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
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

        public async Task<bool> IsCandidateExistsAsync(int id)
        {
            return (await this.dbContext.Candidates.FirstOrDefaultAsync(x => x.Id == id)) != null;
        }

        public async Task<bool> IsCandidateEmailExistsAsync(string email)
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

        public async Task<bool> IsCandidateOwnsEmail(int candidateId, string email)
        {
            var candidateByEmail = await this.dbContext.Candidates.FirstOrDefaultAsync(x => x.Email == email);

            if (candidateByEmail == null)
            {
                return true;
            }

            var candidateById = await this.dbContext.Candidates.FirstOrDefaultAsync(x => x.Id == candidateId);

            return candidateByEmail.Id == candidateById.Id;
        }

        public async Task<Recruiter> GetCandidateRecruiter(int candidateId)
        {
            var candidate = await this.GetByIdAsync(candidateId);
            return await this.recruitersServices.GetRecruiterByIdAsync(candidate.RecruiterId);
        }

        public async Task<int> UpdateAsync(int candidateId, Candidate candidate)
        {
            var entity = await this.dbContext.Candidates.FirstOrDefaultAsync(x => x.Id == candidateId);

            entity.Email = candidate.Email;
            entity.FirstName = candidate.FirstName;
            entity.LastName = candidate.LastName;
            entity.Bio = candidate.Bio;
            entity.BirthDate = candidate.BirthDate;
            entity.RecruiterId = candidate.RecruiterId;

            // Remove all old skills and add all new ones if skills are changed.
            var entitySkillIds = await this.dbContext.CandidateSkills
                                                        .Where(x => x.CandidateId == candidateId)
                                                        .Select(x => x.SkillId)
                                                        .ToArrayAsync();
            var candidateSkillIds = candidate.Skills.Select(x => x.SkillId).ToHashSet();
            var isSkillsChanged = !entitySkillIds.ToHashSet().SetEquals(candidateSkillIds);
            if (isSkillsChanged)
            {
                // Remove all old skills.
                var oldSkills = await this.dbContext.CandidateSkills.Where(x => x.CandidateId == candidateId).ToArrayAsync();
                this.dbContext.CandidateSkills.RemoveRange(oldSkills);
                await this.dbContext.SaveChangesAsync();

                // Add new skills.
                entity.Skills = candidate.Skills.ToArray();
            }

            await this.dbContext.SaveChangesAsync();

            return entity.Id;
        }
    }
}
