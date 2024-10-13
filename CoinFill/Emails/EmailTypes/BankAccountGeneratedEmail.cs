using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Emails.EmailTypes
{
    public class BankAccountGeneratedEmail
    {
        public string BankId { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string BankAccountCurrency { get; set; }
        public string BankAccountEndingDigits { get; set; }
        public string DepositFee { get; set; }
    }
}
