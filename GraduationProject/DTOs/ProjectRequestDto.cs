namespace GraduationProject.DTOs
{
    public class ProjectRequestDto
    {
        public string  Title { get; set; }
        public string Description { get; set; }
        public string LevelOfEducation { get; set; }
        public bool IsAcceptedByProfessor { get; set; }
        public int StudentId { get; set; }
        public int ProfessorId { get; set; }
    }
}
