using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.UserServices.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [RegularExpression(@"^(?=(.*[A-Z]){1,})(?=(.*[a-z]){1,})(?=(.*\d){1,})(?=(.*\W){1,})(?!.*(.)\1{2,}).{6,}$",
           ErrorMessage = "Password must be at least 6 characters long, with at least one uppercase letter, one lowercase letter, one digit, and one special character, and must contain at least two unique characters.")]
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }    
    }
}
