using System;
using System.Collections.Generic;
using System.Text;

namespace CoinFill.Emails.EmailTypes
{
    public class ConfirmationEmail
    {
        public string ToEmail { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
