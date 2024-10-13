using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class UserCards
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Number { get; set; }
        public string ExpirationDate { get; set; }
        public string Cvv { get; set; }
        public decimal Balance { get; set; }
        public string BackgroundImage { get; set; }
        public string PaymentMethodCryptocurrencyId { get; set; }
        public string PaymentMethodCryptocurrencyNetworkId { get; set; }
        public string PaymentWalletAddress { get; set; }
        public int ActivationStatus { get; set; }
        public DateTime? RequestedDateTime { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string FullNameAddress { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
