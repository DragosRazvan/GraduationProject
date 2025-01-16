using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Models
{
    public class StudentModel
    {
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentSecondName { get; set; }
        public string StudentEmail { get; set; }
        public ProjectRequestModel ProjectRequest { get; set; }

    }
}
