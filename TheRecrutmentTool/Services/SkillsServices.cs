namespace TheRecrutmentTool.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using TheRecrutmentTool.Data;
    using TheRecrutmentTool.Data.Models;

    public class SkillsServices : ISkillsServices
    {
        private readonly ApplicationDbContext dbContext;

        public SkillsServices(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> IsSkillExistsAsync(string name)
        {
            return (await this.dbContext.Skills.FirstOrDefaultAsync(x => x.Name == name)) != null;
        }

        public async Task<int> GetSkillIdAsync(string name)
        {
            return (await this.dbContext.Skills.FirstOrDefaultAsync(x => x.Name == name)).Id;
        }

        public async Task<Skill> GetSkillByIdAsync(int skillId)
        {
            return await this.dbContext.Skills.FirstOrDefaultAsync(x => x.Id == skillId);
        }

        public async Task<int> CreateAsync(string name)
        {
            var skill = new Skill()
            {
                Name = name,
            };

            var entity = await this.dbContext.Skills.AddAsync(skill);
            await this.dbContext.SaveChangesAsync();
            return entity.Entity.Id;
        }

        public async Task<int> CreateIfNotExistsAsync(string name)
        {
            var isSkillExists = await this.IsSkillExistsAsync(name);

            if (isSkillExists)
            {
                return await this.GetSkillIdAsync(name);
            }

            return await this.CreateAsync(name);
        }
    }
}
