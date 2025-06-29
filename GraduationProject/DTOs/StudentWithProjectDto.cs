namespace GraduationProject.DTOs
{
    public class StudentWithProjectDto
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string ProjectTitle { get; set; }
        public string? ProjectStatus { get; set; }
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }
    }
}
