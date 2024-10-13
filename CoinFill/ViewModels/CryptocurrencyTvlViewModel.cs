using CoinFill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.ViewModels
{
    public class CryptocurrencyTvlViewModel
    {
        public string CryptocurrencyId { get; set; }
        public string CryptocurrencyName { get; set; }
        public string CryptocurrencyColor { get; set; }
        public string CryptocurrencyIcon { get; set; }
        public int CryptocurrencyOrderNumber { get; set; }
        public double MaxApy { get; set; }
        public string MyTvl { get; set; }
        public string TotalTvl { get; set; }
    }
}
