using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string LevelOfEducation { get; set; } //bachelor or master degree
        public int? ProjectRequestId { get; set; }
        public ProjectRequestModel? ProjectRequest { get; set; }
        public int SpecializationId { get; set; }
        public SpecializationModel Specialization { get; set; }
    }
}
