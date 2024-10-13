using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class GeneratedCryptocurrencyNetworks
    {
        public string UserId { get; set; }
        public string CryptocurrencyId { get; set; }
        public string CryptocurrencyNetworkId { get; set; }
        public DateTime? GenerationDateTime { get; set; }
    }
}
