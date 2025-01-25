using System.ComponentModel.DataAnnotations;
using System.Runtime;

namespace GraduationProject.Models
{
    public class ProjectRequestModel
    {
        public int RequestId { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-z0-9./?]?")]
        public string ProjectTitle { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-z0-9./?]?")]
        public string ProjectDescription { get; set; }
        public List<ProfessorModel> ProfessorsCordinators { get; set; }
        public bool IsAcceptedByProfessor { get; set; }
        public int StudentId { get; set; }
        public int ProfessorId { get; set; }
    }
}
