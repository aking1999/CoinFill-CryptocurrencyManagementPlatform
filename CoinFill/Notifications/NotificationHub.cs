using Microsoft.AspNetCore.SignalR;

namespace CoinFill.Notifications
{
    public class NotificationHub : Hub
    {
        public static string Url { get; } = "notification-hub";
    }
}
