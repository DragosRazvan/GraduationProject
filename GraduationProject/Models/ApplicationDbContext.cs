using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<FacultyModel> Faculties { get; set; }
        public DbSet<SecretaryModel> Secretary { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<SpecializationModel> Specializations { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<ProfessorModel> Professors { get; set; }
        public DbSet<ProjectRequestModel> ProjectRequests { get; set; }
        public DbSet<ProjectIdeaModel> ProfessorsProjectIdeas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships here

            modelBuilder.Entity<DepartmentModel>()
                        .HasOne(o => o.Faculty)
                        .WithMany(c => c.Departments)
                        .HasForeignKey(o => o.FacultyId);

            modelBuilder.Entity<SecretaryModel>()
                        .HasOne(o => o.Faculty)
                        .WithOne(c => c.Secretary)
                        .HasForeignKey<SecretaryModel>(o => o.FacultyId);

            modelBuilder.Entity<SpecializationModel>()
                        .HasOne(o => o.Department)
                        .WithMany(c => c.Specialzations)
                        .HasForeignKey(o => o.DeparmentId);

            modelBuilder.Entity<StudentModel>()
                        .HasOne(o => o.Specialization)
                        .WithMany(c => c.Students)
                        .HasForeignKey(o => o.SpecializationId);

            modelBuilder.Entity<ProfessorModel>()
                        .HasOne(o => o.Department)
                        .WithMany(c => c.Professors)
                        .HasForeignKey(o => o.DepartmentId);

            modelBuilder.Entity<ProjectIdeaModel>()
                        .HasOne(o => o.Professor)
                        .WithMany(c => c.ProjectIdeas)
                        .HasForeignKey(o => o.ProfessorId);

            modelBuilder.Entity<ProjectRequestModel>()
                        .HasOne(o => o.Student)
                        .WithOne(c => c.ProjectRequest)
                        .HasForeignKey<ProjectRequestModel>(o => o.StudentId);

            modelBuilder.Entity<ProjectRequestModel>()
                        .HasOne(o => o.Professor)
                        .WithMany(c => c.CoordinatedProjects)
                        .HasForeignKey(o => o.ProfessorId);

            // Optional safety net (global rule)
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }

    }
}
