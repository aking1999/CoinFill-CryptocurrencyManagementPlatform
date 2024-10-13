using System.ComponentModel.DataAnnotations;

namespace CoinFill.ViewModels.BankAccountTypes
{
    public class AddBankAccountEur
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "First name", Prompt = "John")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string EurAccountHolderFirstName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Last name", Prompt = "Johnson")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string EurAccountHolderLastName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Must be 3 characters.")]
        public string EurAccountCurrency { get; set; }

        [Display(Name = "BIC/SWIFT (optional)", Prompt = "DEUTDEFFXXX")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string EurBicSwift { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "IBAN/Bank account number", Prompt = "DE89370400440532013000")]
        [StringLength(maximumLength: 35, MinimumLength = 7, ErrorMessage = "Enter 7 to 35 characters.")]
        public string EurIBAN { get; set; }
    }
}