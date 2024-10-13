using System.ComponentModel.DataAnnotations;

namespace CoinFill.ViewModels.BankAccountTypes
{
    public class AddBankAccountPln
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "First name", Prompt = "John")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string PlnAccountHolderFirstName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Last name", Prompt = "Johnson")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string PlnAccountHolderLastName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Must be 3 characters.")]
        public string PlnAccountCurrency { get; set; }

        [Display(Name = "BIC/SWIFT (optional)", Prompt = "ERSPPLP1")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string PlnBicSwift { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "IBAN/Bank account number", Prompt = "PL61109010140000071219812874")]
        [StringLength(maximumLength: 35, MinimumLength = 7, ErrorMessage = "Enter 7 to 35 characters.")]
        public string PlnIBAN { get; set; }
    }
}