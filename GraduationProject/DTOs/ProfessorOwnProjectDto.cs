namespace GraduationProject.DTOs
{
    public class ProfessorOwnProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LevelOfEducation { get; set; }
        public int ProfessorId { get; set; }
        public StudentDto? Student { get; set; }
    }
}
