using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<StudentModel> Students { get; set; }
        public DbSet<ProfessorModel> Professors { get; set; }
        public DbSet<SecretaryModel> Secretary { get; set; }
        public DbSet<ProjectRequestModel> ProjectRequests { get; set; }
        public DbSet<ProjectIdeaModel> ProposedProjectIdeas { get; set; }
    }
}
