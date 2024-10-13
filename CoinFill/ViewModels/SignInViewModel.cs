using CoinFill.Helpers.Extensions;
using System.ComponentModel.DataAnnotations;

namespace CoinFill.ViewModels
{
    public class SignInViewModel
    {
        private const string REQUIRED_FIELD = "Field is required.";

        private string email;

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Email", Prompt = "john@gmail.com")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter a valid email address.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Enter 5 to 100 characters.")]
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value.RemoveSpecialCharacters(AllowedSpecialCharacters.ForEmail);
            }
        }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Password", Prompt = "Password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Enter 6 to 100 characters.")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public string ErrorMessage { get; set; }
    }
}
