using CoinFill.Helpers.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CoinFill.ViewModels
{
    public class UserProfileViewModel
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [DataType(DataType.Upload)]
        [Display(Name = "Profile photo")]
        [AllowedFileExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile ProfilePhoto { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool HasCompletedAddress { get; set; }
    }
}
