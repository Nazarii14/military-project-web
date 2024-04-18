using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.Domain.ViewModels.User
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "Enter your email")]
        [MinLength(7, ErrorMessage = "Email must be greater than 7")]
        [MaxLength(50, ErrorMessage = "Email must be less than 50")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be greater than 6")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter your name")]
        [MinLength(1, ErrorMessage = "Name must be greater than 1")]
        [MaxLength(30, ErrorMessage = "Name must be less than 30")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter your lastname")]
        [MinLength(1, ErrorMessage = "Lastname must be greater than 1")]
        [MaxLength(30, ErrorMessage = "Lastname must be less than 30")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Enter your age")]
        [Range(18, 70, ErrorMessage = "Age must be between 18 and 70")]
        public int Age { get; set; }

        [StringLength(6, ErrorMessage = "2FA code must be 6 characters", MinimumLength = 6)]
        public string? TwoFactorSecretKey { get; set; }

        public string? QrCode { get; set; }
    }
}
