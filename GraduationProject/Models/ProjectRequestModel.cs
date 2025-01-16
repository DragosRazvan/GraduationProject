using System.Runtime;

namespace GraduationProject.Models
{
    public class ProjectRequestModel
    {
        public int RequestId { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public List<ProfessorModel> ProfessorsCordinators { get; set; }
        public bool IsAcceptedByProfessor { get; set; }
        public int StudentId { get; set; }
        public int ProfessorId { get; set; }
    }
}
