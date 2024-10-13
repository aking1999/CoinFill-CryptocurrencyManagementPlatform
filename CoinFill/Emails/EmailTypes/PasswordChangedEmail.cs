using System;
using System.Collections.Generic;
using System.Text;

namespace CoinFill.Emails.EmailTypes
{
    public class PasswordChangedEmail
    {
        public string ToEmail { get; set; }
        public string FirstName { get; set; }
    }
}
