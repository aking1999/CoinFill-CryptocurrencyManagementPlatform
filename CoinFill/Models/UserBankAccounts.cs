using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class UserBankAccounts
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Currency { get; set; }
        public string BankAccountNumber { get; set; }
        public string BicSwift { get; set; }
        public string RoutingNumber { get; set; }
        public string TransitNumber { get; set; }
        public string InstitutionNumber { get; set; }
        public DateTime? AddedDateTime { get; set; }
    }
}
