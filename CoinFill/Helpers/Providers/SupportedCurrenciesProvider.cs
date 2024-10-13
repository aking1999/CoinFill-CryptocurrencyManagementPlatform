using CoinFill.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Helpers.Providers
{
    public static class SupportedCurrenciesProvider
    {
        public static List<Currency> SupportedCurrencies { get; } = new List<Currency>
        {   new Currency { Name = "", ShortName = "EUR" },
            new Currency { Name = "", ShortName = "USD" },
            new Currency { Name = "", ShortName = "GBP" },
            new Currency { Name = "", ShortName = "CHF" },
            new Currency { Name = "", ShortName = "CNY" },
            new Currency { Name = "", ShortName = "HKD" },
            new Currency { Name = "", ShortName = "RUB" },
            new Currency { Name = "", ShortName = "IRR" },
            new Currency { Name = "", ShortName = "KRW" },
            new Currency { Name = "", ShortName = "CAD" },
            new Currency { Name = "", ShortName = "AUD" },
            new Currency { Name = "", ShortName = "NOK" },
            new Currency { Name = "", ShortName = "DKK" },
            new Currency { Name = "", ShortName = "SEK" },
            new Currency { Name = "", ShortName = "INR" },
            new Currency { Name = "", ShortName = "HUF" },
            new Currency { Name = "", ShortName = "RSD" },
            new Currency { Name = "", ShortName = "BAM" },
            new Currency { Name = "", ShortName = "PLN" },
            new Currency { Name = "", ShortName = "UAH" },
            new Currency { Name = "", ShortName = "CZK" },
            new Currency { Name = "", ShortName = "RON" },
            new Currency { Name = "", ShortName = "BGN" },
            new Currency { Name = "", ShortName = "TRY" },
            new Currency { Name = "", ShortName = "AED" },
            new Currency { Name = "", ShortName = "MXN" },
            new Currency { Name = "", ShortName = "ILS" },
            new Currency { Name = "", ShortName = "PKR" },
            new Currency { Name = "", ShortName = "MAD" },
        };
    }
}
