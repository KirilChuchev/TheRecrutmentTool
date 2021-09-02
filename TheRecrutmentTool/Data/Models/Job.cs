﻿namespace TheRecrutmentTool.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Job
    {
        public Job()
        {
            this.Skills = new HashSet<Skill>();
            this.Interviews = new HashSet<Interview>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title should not be empty.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description should not be empty.")]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Salary { get; set; }
        public ICollection<Skill> Skills { get; set; }

        public ICollection<Interview> Interviews { get; set; }
    }
}
