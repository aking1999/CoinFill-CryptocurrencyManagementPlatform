using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoinFill.Models
{
    public partial class Notifications
    {
        public string Id { get; set; }
        public string ReceiverUserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Severity { get; set; }
        public string Icon { get; set; }
        public bool? Read { get; set; }
        public bool? Important { get; set; }
        public DateTime? SendingDateTime { get; set; }

        public virtual AspNetUsers ReceiverUser { get; set; }
    }
}
