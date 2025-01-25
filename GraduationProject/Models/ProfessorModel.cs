namespace GraduationProject.Models
{
    public class ProfessorModel
    {
        public int ProfessorId { get; set; }
        public string ProfessorFirstName { get; set; }
        public string ProfessorSecondName { get; set; }
        public string ProfessorEmail { get; set; }
        public List<int> SpecializationId { get; set; }
        public List<ProjectRequestModel> CoordinatedProjects { get; set; }
    }
}
