namespace TheRecrutmentTool.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class JobSkill
    {
        [Key]
        public int Id { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        public int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}
