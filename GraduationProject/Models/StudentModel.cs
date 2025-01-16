using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Models
{
    public class StudentModel
    {
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentSecondName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "Email address should have maximum 100 characters")]
        [RegularExpression(@"^[a-zA-z0-9@.]{11,}$", ErrorMessage = "Email address should contain only letters, numbers and the characters @ and .")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9?_/#]{8,}$", ErrorMessage = "Password can only contain letters, numbers and the characters ?, _, /, #")]
        public string Password { get; set; }

    }
}
