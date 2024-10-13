using System.Collections.Generic;

namespace CoinFill.ViewModels
{
    public class CryptocurrencyValidatorsViewModel
    {
        public string CryptocurrencyId { get; set; }
        public string CryptocurrencyName { get; set; }
        public string CryptocurrencyColor { get; set; }
        public string CryptocurrencyIcon { get; set; }
        public int CryptocurrencyOrderNumber { get; set; }
        public double MaxApy { get; set; }
        public List<ValidatorShowcaseViewModel> Validators { get; set; }

        public CryptocurrencyValidatorsViewModel()
        {
            Validators = new List<ValidatorShowcaseViewModel>();
        }
    }
}
