namespace TheRecrutmentTool.Data
{
    using Microsoft.EntityFrameworkCore;
    using TheRecrutmentTool.Data.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<Interview> Interviews { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Recruiter> Recruiters { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<CandidateSkill> CandidateSkills { get; set; }
        public virtual DbSet<JobSkill> JobSkills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConfiguration.ConnectionString);
            }


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Recruiter>()
                .HasMany(e => e.Interviews)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
