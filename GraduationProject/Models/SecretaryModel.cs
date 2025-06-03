namespace GraduationProject.Models
{
    public class SecretaryModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public int FacultyId { get; set; }
        public FacultyModel Faculty { get; set; }
    }
}
