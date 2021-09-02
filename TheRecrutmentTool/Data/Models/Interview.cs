using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheRecrutmentTool.Data.Models
{
    public class Interview
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Candidate should not be empty.")]
        //[ForeignKey("CandidateId")]
        public int CandidateId { get; set; }
        //public Candidate Candidate { get; set; }

        [Required(ErrorMessage = "Recruiter should not be empty.")]
        [ForeignKey("RecruiterId")]
        public int RecruiterId { get; set; }
        //public Recruiter Recruiter { get; set; }

        [Required(ErrorMessage = "Job should not be empty.")]
        //[ForeignKey("JobId")]
        public int JobId { get; set; }
        //public Job Job { get; set; }
    }
}
