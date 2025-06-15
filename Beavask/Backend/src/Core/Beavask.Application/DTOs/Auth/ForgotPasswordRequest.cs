using System.ComponentModel.DataAnnotations;

namespace Beavask.Application.DTOs.Auth
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}

