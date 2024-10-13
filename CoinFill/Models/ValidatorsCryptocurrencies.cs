using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class ValidatorsCryptocurrencies
    {
        public string ValidatorId { get; set; }
        public string CryptocurrencyId { get; set; }
        public double Apy { get; set; }
        public double TotalStaked { get; set; }
        public double UnlockTimeHours { get; set; }
        public double MinimumDepositAmount { get; set; }

        public virtual Cryptocurrencies Cryptocurrency { get; set; }
        public virtual Validators Validator { get; set; }
    }
}
