namespace TheRecrutmentTool.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using TheRecrutmentTool.Data;

    public class SkillsServices : ISkillsServices
    {
        private readonly ApplicationDbContext dbContext;

        public SkillsServices(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> IsSkillExists(string name)
        {
            return (await this.dbContext.Skills.FirstOrDefaultAsync(x => x.Name == name)) != null;
        }
    }
}
