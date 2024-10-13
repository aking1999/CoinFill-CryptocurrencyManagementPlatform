using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels.BankAccountTypes
{
    public class AddBankAccountAed
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "First name", Prompt = "John")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string AedAccountHolderFirstName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Last name", Prompt = "Johnson")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string AedAccountHolderLastName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Must be 3 characters.")]
        public string AedAccountCurrency { get; set; }

        [Display(Name = "BIC/SWIFT (optional)", Prompt = "ADCBAEAA")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string AedBicSwift { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "IBAN/Bank account number", Prompt = "AE070331234567890123456")]
        [StringLength(maximumLength: 35, MinimumLength = 7, ErrorMessage = "Enter 7 to 35 characters.")]
        public string AedIBAN { get; set; }
    }
}
