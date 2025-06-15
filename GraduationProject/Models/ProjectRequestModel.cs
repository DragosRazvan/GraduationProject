using System.ComponentModel.DataAnnotations;
using System.Runtime;

namespace GraduationProject.Models
{
    public class ProjectRequestModel
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-z0-9./?]?")]
        public string Title { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-z0-9./?]?")]
        public string Description { get; set; }
        public string LevelOfEducation { get; set; } //bachelor or master degree
        public bool IsAcceptedByProfessor { get; set; }
        public int? StudentId { get; set; }
        public StudentModel? Student { get; set; }
        public int ProfessorId { get; set; }
        public ProfessorModel Professor { get; set; }
    }
}
