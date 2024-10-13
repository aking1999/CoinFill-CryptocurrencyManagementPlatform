using System.ComponentModel.DataAnnotations;

namespace CoinFill.PartialViewModels
{
    public class PaymentWalletAddressPartialViewModel
    {
        public string CryptocurrencyName { get; set; }

        public string PaymentId { get; set; }

        [Display(Name = "Your wallet address")]
        [Required(ErrorMessage = "Field is required.")]
        [StringLength(1023, MinimumLength = 6, ErrorMessage = "Enter 6 to 1023 characters.")]
        public string WalletAddress { get; set; }
    }
}
