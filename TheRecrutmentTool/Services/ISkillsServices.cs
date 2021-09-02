namespace TheRecrutmentTool.Services
{
    using System.Threading.Tasks;

    public interface ISkillsServices
    {
        Task<bool> IsSkillExists(string name);
    }
}
