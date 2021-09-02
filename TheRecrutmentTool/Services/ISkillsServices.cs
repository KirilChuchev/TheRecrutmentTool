namespace TheRecrutmentTool.Services
{
    using System.Threading.Tasks;
    using TheRecrutmentTool.Data.Models;

    public interface ISkillsServices
    {
        Task<bool> IsSkillExistsAsync(string name);

        Task<int> GetSkillIdAsync(string name);

        Task<int> CreateAsync(string name);

        Task<int> CreateIfNotExistsAsync(string name);

        Task<Skill> GetSkillByIdAsync(int skillId);
    }
}
