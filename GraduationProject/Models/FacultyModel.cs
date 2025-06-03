namespace GraduationProject.Models
{
    public class FacultyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<DepartmentModel> Departments { get; set; }
        public SecretaryModel Secretary { get; set; }
    }
}
