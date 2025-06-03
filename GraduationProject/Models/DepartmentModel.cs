namespace GraduationProject.Models
{
    public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<SpecializationModel> Specialzations { get; set; }
        public ICollection<ProfessorModel> Professors { get; set; }
        public int FacultyId { get; set; }
        public FacultyModel Faculty { get; set; }
    }
}
