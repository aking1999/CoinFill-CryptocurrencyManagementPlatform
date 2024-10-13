using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class CryptocurrencyNetworks
    {
        public string Id { get; set; }
        public string CryptocurrencyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string QrImage { get; set; }
        public int OrderNumber { get; set; }
        public int UsedCount { get; set; }

        public virtual Cryptocurrencies Cryptocurrency { get; set; }
    }
}
