using GraduationProject.Models;

namespace GraduationProject.DTOs
{
    public class ProfessorDto
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public int? NumberOfCoordinatedProjects { get; set; }
        public int DepartmentId { get; set; }
    }
}
