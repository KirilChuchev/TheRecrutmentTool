namespace TheRecrutmentTool.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Candidate
    {
        public Candidate()
        {
            this.Skills = new HashSet<Skill>();
            this.Interviews = new HashSet<Interview>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "FirstName should not be empty.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName should not be empty.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email should not be empty.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bio should not be empty.")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "BirthDate should not be empty.")]
        public DateTime BirthDate { get; set; }

        public ICollection<Skill> Skills { get; set; }

        [Required(ErrorMessage = "Recruiter should not be empty.")]
        [ForeignKey("RecruiterId")]
        public int RecruiterId { get; set; }
        public Recruiter Recruiter { get; set; }

        public ICollection<Interview> Interviews { get; set; }
    }
}
