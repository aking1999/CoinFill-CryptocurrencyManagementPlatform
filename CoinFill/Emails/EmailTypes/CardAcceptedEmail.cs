using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Emails.EmailTypes
{
    public class CardAcceptedEmail
    {
        public string ToEmail { get; set; }
        public string CardId { get; set; }
        public string CardType { get; set; }
        public string CardBrand { get; set; }
        public string CardNumberEndingDigits { get; set; }
        public string CoinNameUsedForPayment { get; set; }
    }
}
