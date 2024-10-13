using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class Payments
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Reason { get; set; }
        public string ReasonId { get; set; }
        public decimal AmountShouldBe { get; set; }
        public string PaymentMethodCryptocurrencyId { get; set; }
        public string PaymentMethodCryptocurrencyNetworkId { get; set; }
        public int ActivationStatus { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
    }
}
