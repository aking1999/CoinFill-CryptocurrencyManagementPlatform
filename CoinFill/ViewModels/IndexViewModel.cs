using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels
{
    public class IndexViewModel
    {
        public int SupportedCountriesCount { get; set; }
        public int SupportedCurrenciesCount { get; set; }
        public int SupportedCryptocurrencies { get; set; }
        public NewsletterSubscriptionViewModel Newsletter { get; set; }

        public IndexViewModel()
        {
            Newsletter = new NewsletterSubscriptionViewModel();
        }
    }
}
