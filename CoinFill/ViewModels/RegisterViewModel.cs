using CoinFill.Helpers.Extensions;
using System.ComponentModel.DataAnnotations;

namespace CoinFill.ViewModels
{
    public class RegisterViewModel
    {
        private const string REQUIRED_FIELD = "Field is required.";

        private string firstName;
        private string lastName;
        private string email;

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "First name", Prompt = "First name")]
        [StringLength(maximumLength: 32, ErrorMessage = "Enter up to 32 characters.")]
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value.RemoveSpecialCharacters();
            }
        }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Last name", Prompt = "Last name")]
        [StringLength(maximumLength: 32, ErrorMessage = "Enter up to 32 characters.")]
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value.RemoveSpecialCharacters();
            }
        }

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

        [DataType(DataType.Password)]
        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Confirm password", Prompt = "Confirm password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Enter 6 to 100 characters.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
