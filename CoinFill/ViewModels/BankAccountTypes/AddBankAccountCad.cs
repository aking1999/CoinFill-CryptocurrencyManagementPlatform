using System.ComponentModel.DataAnnotations;

namespace CoinFill.ViewModels.BankAccountTypes
{
    public class AddBankAccountCad
    {
        private const string REQUIRED_FIELD = "Field is required.";

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "First name", Prompt = "John")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string CadAccountHolderFirstName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Last name", Prompt = "Johnson")]
        [StringLength(maximumLength: 16, ErrorMessage = "Enter up to 16 characters.")]
        public string CadAccountHolderLastName { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Must be 3 characters.")]
        public string CadAccountCurrency { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Transit number", Prompt = "12345")]
        [StringLength(maximumLength: 6, MinimumLength = 4, ErrorMessage = "Enter 4 to 6 characters.")]
        public string CadTransitNumber { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Institution number", Prompt = "621")]
        [StringLength(maximumLength: 3, MinimumLength = 3, ErrorMessage = "Must be 3 characters.")]
        public string CadInstitutionNumber { get; set; }

        [Required(ErrorMessage = REQUIRED_FIELD)]
        [Display(Name = "Bank account number", Prompt = "4092643")]
        [StringLength(maximumLength: 13, MinimumLength = 7, ErrorMessage = "Enter 7 to 12 characters.")]
        public string CadBankAccountNumber { get; set; }
    }
}
