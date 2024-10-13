using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Emails.EmailTypes
{
    public class NetworkAddressGeneratedEmail
    {
        public string ToEmail { get; set; }
        public string CoinName { get; set; }
        public string NetworkName { get; set; }
        public string Address { get; set; }
    }
}
