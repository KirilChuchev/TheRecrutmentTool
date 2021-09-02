namespace TheRecrutmentTool.Services
{
    using System.Threading.Tasks;
    using TheRecrutmentTool.Data.Models;

    public interface IRecruitersServices
    {
        Task<bool> IsRecruiterExistsAsync(string email);

        Task<int> GetRecruiterIdAsync(string email);

        Task<int> CreateAsync(Recruiter recruiter);

        Task<int> CreateIfNotExistsAsync(Recruiter recruiter);
    }
}
