using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class NewsletterSubscribers
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public int NotifiedCount { get; set; }
        public string IpAddress { get; set; }
        public DateTime? LastNotifiedDateTime { get; set; }
        public DateTime? SubscribeDateTime { get; set; }
        public bool? Verified { get; set; }
        public long VerificationCount { get; set; }
    }
}
