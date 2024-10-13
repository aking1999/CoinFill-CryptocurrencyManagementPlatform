using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class UserActivityLogs
    {
        public string Id { get; set; }
        public string UserIdOrAnonymous { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string QueryDataJson { get; set; }
        public string MethodType { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime? ActivityDateTime { get; set; }
        public bool? IsCrawler { get; set; }
    }
}
