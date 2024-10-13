using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels
{
    public class RpcEmail
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Email", Prompt = "john@gmail.com")]
        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter a valid email.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Enter 5 to 100 characters.")]
        public string ToEmail { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "Enter 5 to 128 characters.")]
        [Display(Name = "Subject", Prompt = "Email subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(128, MinimumLength = 5, ErrorMessage = "Enter 5 to 128 characters.")]
        [Display(Name = "Inner email header", Prompt = "Inner email header right above body")]
        public string InnerEmailHeader { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(2047, MinimumLength = 5, ErrorMessage = "Enter 5 to 2047 characters.")]
        [Display(Name = "Body", Prompt = "Email body")]
        public string Body { get; set; }
    }
}
