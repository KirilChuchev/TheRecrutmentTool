
namespace TheRecrutmentTool.Services
{
    using System.Threading.Tasks;
    using TheRecrutmentTool.ViewModels.Candidate;

    public interface ICandidatesServices
    {
        Task<bool> IsCandidateExists(string email);

        Task<int> Create(CandidateCreateModel model);
    }
}
