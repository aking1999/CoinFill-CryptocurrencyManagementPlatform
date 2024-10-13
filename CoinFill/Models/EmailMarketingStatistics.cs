using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class EmailMarketingStatistics
    {
        public string Id { get; set; }
        public string CampaignName { get; set; }
        public string Email { get; set; }
        public int OpenCount { get; set; }
        public bool? Unsubscribed { get; set; }
        public DateTime? LastOpenedDateTime { get; set; }
        public string IpAddress { get; set; }
    }
}
