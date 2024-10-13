using CoinFill.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinFill.Helpers.Providers
{
    public class CustomerSupportTicketTopicsProvider
    {
        public static ClientSupportTicketTopics Sales => new ClientSupportTicketTopics { Id = "sales", Name = "Sales agent", Color = "#2ba0ff", Icon = "far fa-building" };
        public static ClientSupportTicketTopics Support => new ClientSupportTicketTopics { Id = "support", Name = "Support agent", Color = "#2ba0ff", Icon = "far fa-comments" };
        public static ClientSupportTicketTopics Marketing => new ClientSupportTicketTopics { Id = "marketing", Name = "Marketing agent", Color = "#2ba0ff", Icon = "far fa-newspaper" };
        public static ClientSupportTicketTopics Payment => new ClientSupportTicketTopics { Id = "payment", Name = "Payment", Color = "#00d27a", Icon = "far fa-usd-circle" };
        public static ClientSupportTicketTopics ReportError => new ClientSupportTicketTopics { Id = "report-error", Name = "Report an error", Color = "#000", Icon = "far fa-bug" };
        public static ClientSupportTicketTopics AskQuestion => new ClientSupportTicketTopics { Id = "ask-question", Name = "Ask a general question", Color = "#27bcfd", Icon = "far fa-question" };
        public static ClientSupportTicketTopics GiveFeedback => new ClientSupportTicketTopics { Id = "feedback", Name = "Send feedback", Color = "#34a4ff", Icon = "far fa-comment-alt-lines" };
        public static ClientSupportTicketTopics RequestAccountDeletion => new ClientSupportTicketTopics { Id = "account-deletion-request", Name = "Request account deletion", Color = "#e62e52", Icon = "fas fa-times" };
    }
}
