using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels.BankAccountTypes
{
    public class AddBankAccountInr
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "First name", Prompt = "John")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string InrAccountHolderFirstName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Last name", Prompt = "Johnson")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string InrAccountHolderLastName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Must be 3 characters.")]
        public string InrAccountCurrency { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "IFSC", Prompt = "SBIN0005943")]
        [StringLength(maximumLength: 17, ErrorMessage = "Enter up to 17 characters.")]
        public string InrIFSC { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Bank account number", Prompt = "00000062070957238")]
        [StringLength(maximumLength: 35, MinimumLength = 7, ErrorMessage = "Enter 7 to 35 characters.")]
        public string InrBankAccountNumber { get; set; }
    }
}
