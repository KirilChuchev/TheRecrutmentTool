
namespace TheRecrutmentTool.Services
{
    using System.Threading.Tasks;
    using TheRecrutmentTool.Data.Models;

    public interface ICandidatesServices
    {
        Task<bool> IsCandidateExistsAsync(string email);

        Task<int> CreateAsync(Candidate candidate);

        Task<Candidate> GetByIdAsync(int id);
    }
}
