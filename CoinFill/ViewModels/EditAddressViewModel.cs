using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels
{
    public class EditAddressViewModel
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Full name", Prompt = "Recipient's name")]
        [StringLength(32, ErrorMessage = "Enter up to 32 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Street", Prompt = "Street name")]
        [StringLength(32, ErrorMessage = "Enter up to 32 characters.")]
        public string Street { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "House number", Prompt = "House number")]
        [StringLength(32, ErrorMessage = "Enter up to 32 characters.")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "City", Prompt = "City")]
        [StringLength(32, ErrorMessage = "Enter up to 32 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Postal code", Prompt = "Postal code")]
        [StringLength(10, ErrorMessage = "Enter up to 10 characters.")]
        public string PostalCode { get; set; }

        [Display(Name = "Choose your country", Prompt = "Country")]
        [Required(ErrorMessage = REQUIRED_FIELD)]
        public string Chosen_CountryId { get; set; }

        public List<SelectListItem> ToChooseFrom_Countries { get; set; }

        public EditAddressViewModel()
        {
            ToChooseFrom_Countries = new List<SelectListItem>();
        }
    }
}
