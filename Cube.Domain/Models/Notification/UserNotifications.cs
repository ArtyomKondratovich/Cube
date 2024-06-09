using Cube.Domain.Entities;

namespace Cube.Domain.Models.Notification
{
    public class UserNotifications
    {
        public IEnumerable<NotificationEntity>? Notifications { get; set; }
    }
}
