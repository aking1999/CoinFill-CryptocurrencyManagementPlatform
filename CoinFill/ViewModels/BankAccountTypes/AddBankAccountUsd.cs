using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels.BankAccountTypes
{
    public class AddBankAccountUsd
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "First name", Prompt = "John")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string UsdAccountHolderFirstName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Last name", Prompt = "Johnson")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string UsdAccountHolderLastName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Must be 3 characters.")]
        public string UsdAccountCurrency { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "RoutingNumber", Prompt = "999888777")]
        [StringLength(maximumLength: 9, MinimumLength = 7, ErrorMessage = "Enter 7 to 9 characters.")]
        public string UsdRoutingNumber { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Bank account number", Prompt = "000123456789")]
        [StringLength(maximumLength: 13, MinimumLength = 11, ErrorMessage = "Enter 11 to 13 characters.")]
        public string UsdBankAccountNumber { get; set; }
    }
}
