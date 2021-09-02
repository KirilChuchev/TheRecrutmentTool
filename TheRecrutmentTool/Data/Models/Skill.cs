namespace TheRecrutmentTool.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Skill
    {
        public Skill()
        {
            this.Candidates = new HashSet<Candidate>();
            this.Jobs = new HashSet<Job>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Skill name should not be empty.")]
        public string Name { get; set; }

        public ICollection<Candidate> Candidates { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }
}
