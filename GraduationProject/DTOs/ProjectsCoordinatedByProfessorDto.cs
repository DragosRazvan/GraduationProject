using Microsoft.AspNetCore.Components.Web;

namespace GraduationProject.DTOs
{
    public class ProjectsCoordinatedByProfessorDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LevelOfEducation { get; set; }
        public bool IsAcceptedByProfessor { get; set; }
        public string Status { get; set; }
        public int ProfessorId { get; set; }
        public StudentDto? Student { get; set; }
    }
}
