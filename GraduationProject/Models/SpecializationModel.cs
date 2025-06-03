namespace GraduationProject.Models
{
    public class SpecializationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LevelOfEducation { get; set; } //bachelor or master degree
        public ICollection<StudentModel> Students { get; set; }
        public int DeparmentId { get; set; }
        public DepartmentModel Department { get; set; }
    }
}
