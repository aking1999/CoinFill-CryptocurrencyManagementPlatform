using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels
{
    public class PayPalViewModel
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Recipient's PayPal email", Prompt = "john@gmail.com")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter a valid email address.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Enter 5 to 100 characters.")]
        public string PayPalRecipientEmail { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]

        public string PayPalTransferType { get; set; }
    }
}
