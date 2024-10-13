using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels
{
    public class CustomerSupportViewModel
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Email", Prompt = "john@gmail.com")]
        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter a valid email.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Enter 5 to 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(2047, MinimumLength = 10, ErrorMessage = "Enter 10 to 2047 characters.")]
        [Display(Name = "Your query", Prompt = "Write your query or feedback here...")]
        public string Text { get; set; }

        [Display(Name = "Topic", Prompt = "Topic")]
        [Required(ErrorMessage = REQUIRED_FIELD)]
        public string Chosen_TopicId { get; set; }

        public List<SelectListItem> ToChooseFrom_Topics { get; set; }

        public CustomerSupportViewModel()
        {
            ToChooseFrom_Topics = new List<SelectListItem>();
        }
    }
}
