﻿namespace CoinFill.Helpers.Models
{
    public class Toast
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public string Severity { get; set; }

        public Toast(string header, string body, string severity)
        {
            Header = header;
            Body = body;
            Severity = severity;
        }
    }
}
