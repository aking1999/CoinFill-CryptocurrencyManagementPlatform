using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels
{
    public class StakingViewModel
    {
        public int MyStake { get; } = 0;

        public List<CryptocurrencyValidatorsViewModel> Cryptocurrencies { get; set; }
        public NewsletterSubscriptionViewModel Newsletter { get; set; }

        public StakingViewModel()
        {
            Cryptocurrencies = new List<CryptocurrencyValidatorsViewModel>();
            Newsletter = new NewsletterSubscriptionViewModel();
        }
    }
}
