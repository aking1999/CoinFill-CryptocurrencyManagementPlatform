using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class Validators
    {
        public Validators()
        {
            ValidatorsCryptocurrencies = new HashSet<ValidatorsCryptocurrencies>();
        }

        public string Id { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
        public int AvailableCryptocurrenciesToStake { get; set; }

        public virtual ICollection<ValidatorsCryptocurrencies> ValidatorsCryptocurrencies { get; set; }
    }
}
