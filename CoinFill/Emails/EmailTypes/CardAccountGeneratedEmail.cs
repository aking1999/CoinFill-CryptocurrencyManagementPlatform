using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Emails.EmailTypes
{
    public class CardAccountGeneratedEmail
    {
        public string CardId { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string CardAccountCurrency { get; set; }
        public string CardAccountEndingDigits { get; set; }
        public string DepositFee { get; set; }
    }
}