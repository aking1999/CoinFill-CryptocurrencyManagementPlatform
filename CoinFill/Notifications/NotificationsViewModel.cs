using System;

namespace CoinFill.Notifications
{
    public class NotificationsViewModel
    {
        public string Id { get; set; }
        public string ReceiverUserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Severity { get; set; }
        public bool? Read { get; set; }
        public string Icon { get; set; }
        public bool? Important { get; set; }

        public NotificationsViewModel()
        {
        }

        public void Map(CoinFill.Models.Notifications notification)
        {
            Id = notification.Id;
            ReceiverUserId = notification.ReceiverUserId;
            Title = notification.Title;
            Body = notification.Body;
            Severity = notification.Severity;
            Read = notification.Read;
            Important = notification.Important;
            Icon = notification.Icon;
        }
    }
}
