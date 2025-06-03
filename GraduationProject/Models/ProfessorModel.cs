namespace GraduationProject.Models
{
    public class ProfessorModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public int? NumberOfCoordinatedProjects { get; set; }
        public ICollection<ProjectRequestModel>? CoordinatedProjects { get; set; }
        public ICollection<ProjectIdeaModel>? ProjectIdeas { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentModel Department { get; set; }
    }
}
