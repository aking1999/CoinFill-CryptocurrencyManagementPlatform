using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class Cryptocurrencies
    {
        public Cryptocurrencies()
        {
            CryptocurrencyNetworks = new HashSet<CryptocurrencyNetworks>();
            ValidatorsCryptocurrencies = new HashSet<ValidatorsCryptocurrencies>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public int OrderNumber { get; set; }
        public double MinimumTransactionAmount { get; set; }

        public virtual ICollection<CryptocurrencyNetworks> CryptocurrencyNetworks { get; set; }
        public virtual ICollection<ValidatorsCryptocurrencies> ValidatorsCryptocurrencies { get; set; }
    }
}
