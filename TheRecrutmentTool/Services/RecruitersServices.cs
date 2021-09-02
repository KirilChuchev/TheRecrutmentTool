using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TheRecrutmentTool.Data;
using TheRecrutmentTool.Data.Models;
using TheRecrutmentTool.ViewModels.Recruiter;

namespace TheRecrutmentTool.Services
{
    public class RecruitersServices : IRecruitersServices
    {
        private readonly ApplicationDbContext dbContext;

        public RecruitersServices(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> IsRecruiterExists(string email)
        {
            return (await this.dbContext.Recruiters.FirstOrDefaultAsync(x => x.Email == email)) != null;
        }

        public async Task<int> GetRecruiterId(string email)
        {
            return (await this.dbContext.Recruiters.FirstOrDefaultAsync(x => x.Email == email)).Id;
        }

        public async Task<int> Create(RecruiterCreateModel model)
        {
            var newRecruiter = new Recruiter()
            {
                LastName = model.LastName,
                Email = model.Email,
                Country = model.Country,
            };

            var entity = await this.dbContext.AddAsync(newRecruiter);
            await this.dbContext.SaveChangesAsync();

            return entity.Entity.Id;
        }
    }
}
