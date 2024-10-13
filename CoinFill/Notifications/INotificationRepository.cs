using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoinFill.Notifications
{
    public interface INotificationRepository
    {
        List<NotificationsViewModel> GetAll(string userId);
        List<NotificationsViewModel> GetUnread(string userId);
        Task SendAsync(CoinFill.Models.Notifications notification);
        Task SendAsync(NotificationTypes.NotificationForRole notification, string userRole);
        Task ReadAsync(string notificationId, string userId);
        Task ImportantAsync(string notificationId, string userId);
        Task UnimportantAsync(string notificationId, string userId);
    }
}
