using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class SignUpViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="Invalid format for Email")]
        public string Email { get; set; }
        [Required]
        [MaxLength(6)]
        public string Password { get; set; }
        [Required]
        [MaxLength(6)]
        [Compare(nameof(Password) ,ErrorMessage ="Password MisMatch")]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
