using CoinFill.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels
{
    public class NewsletterSubscriptionViewModel
    {
        private string email;

        [Display(Name = "Email", Prompt = "john@gmail.com")]
        [Required(ErrorMessage = "Field is required.")]
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
    }
}
