using System.ComponentModel.DataAnnotations;

namespace CoinFill.ViewModels.BankAccountTypes
{
    public class AddBankAccountRon
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "First name", Prompt = "John")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string RonAccountHolderFirstName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Last name", Prompt = "Johnson")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string RonAccountHolderLastName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Must be 3 characters.")]
        public string RonAccountCurrency { get; set; }

        [Display(Name = "BIC/SWIFT (optional)", Prompt = "TNISROB2")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string RonBicSwift { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "IBAN/Bank account number", Prompt = "RO49AAAA1B31007593840000")]
        [StringLength(maximumLength: 35, MinimumLength = 7, ErrorMessage = "Enter 7 to 35 characters.")]
        public string RonIBAN { get; set; }
    }
}
