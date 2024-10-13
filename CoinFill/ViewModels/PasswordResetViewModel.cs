using System.ComponentModel.DataAnnotations;

namespace CoinFill.ViewModels
{
    public class PasswordResetViewModel
    {
        private const string FIELD_REQUIRED = "Field is required.";

        public string Uid { get; set; }
        public string Token { get; set; }

        public string ProfilePhoto { get; set; }
        public string FullName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = FIELD_REQUIRED)]
        [Display(Name = "New password", Prompt = "Password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Enter 6 to 100 characters.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = FIELD_REQUIRED)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm password", Prompt = "Confirm password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Enter 6 to 100 characters.")]
        public string ConfirmPassword { get; set; }
    }
}
