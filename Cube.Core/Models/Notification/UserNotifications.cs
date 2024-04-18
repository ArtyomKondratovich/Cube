using Cube.Core.Entities;

namespace Cube.Core.Models.Notification
{
    public class UserNotifications
    {
        public IEnumerable<NotificationEntity>? Notifications { get; set; }
    }
}
