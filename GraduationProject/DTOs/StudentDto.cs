namespace GraduationProject.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string LevelOfEducation { get; set; }
        public int? ProjectRequestId { get; set; }
        public int SpecializationId { get; set; }
    }
}
