namespace TheRecrutmentTool.Services
{
    using System.Threading.Tasks;
    using TheRecrutmentTool.ViewModels.Recruiter;

    public interface IRecruitersServices
    {
        Task<bool> IsRecruiterExists(string email);

        Task<int> GetRecruiterId(string email);

        Task<int> Create(RecruiterCreateModel model);
    }
}
