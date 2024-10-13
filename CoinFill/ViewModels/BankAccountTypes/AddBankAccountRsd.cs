using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels.BankAccountTypes
{
    public class AddBankAccountRsd
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "First name", Prompt = "John")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string RsdAccountHolderFirstName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Last name", Prompt = "Johnson")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string RsdAccountHolderLastName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Must be 3 characters.")]
        public string RsdAccountCurrency { get; set; }

        [Display(Name = "BIC/SWIFT (optional)", Prompt = "RZBSRSBG")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string RsdBicSwift { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "IBAN/Bank account number", Prompt = "RS35265719337385001284")]
        [StringLength(maximumLength: 35, MinimumLength = 7, ErrorMessage = "Enter 7 to 35 characters.")]
        public string RsdIBAN { get; set; }
    }
}
