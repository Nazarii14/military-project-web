using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter your email")]
        [MinLength(7, ErrorMessage = "Email must be greater than 7")]
        [MaxLength(50, ErrorMessage = "Email must be less than 50")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be greater than 6")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter the 2FA code")]
        [StringLength(6, ErrorMessage = "2FA code must be 6 characters", MinimumLength = 6)]
        public string TwoFactorSecretKey { get; set; }
    }
}
