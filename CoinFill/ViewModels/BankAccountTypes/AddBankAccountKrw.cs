using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels.BankAccountTypes
{
    public class AddBankAccountKrw
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "First name", Prompt = "John")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string KrwAccountHolderFirstName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Last name", Prompt = "Johnson")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string KrwAccountHolderLastName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Must be 3 characters.")]
        public string KrwAccountCurrency { get; set; }

        [Display(Name = "Bank name/Code (optional)", Prompt = "001")]
        [StringLength(maximumLength: 128, ErrorMessage = "Enter up to 128 characters.")]
        public string KrwBankCode { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Bank account number", Prompt = "760737-04-006609")]
        [StringLength(maximumLength: 35, MinimumLength = 7, ErrorMessage = "Enter 7 to 35 characters.")]
        public string KrwBankAccountNumber { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Phone number", Prompt = "8201012345678")]
        [StringLength(maximumLength: 18, MinimumLength = 4, ErrorMessage = "Enter 4 to 18 characters.")]
        public string KrwPhoneNumber { get; set; }
    }
}
