using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Emails.EmailTypes
{
    public class CaptureTransactionEmail
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
    }
}