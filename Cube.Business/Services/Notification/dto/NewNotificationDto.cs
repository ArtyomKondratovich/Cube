using Cube.Domain.Enums;

namespace Cube.Business.Services.Notification.dto
{
    public class NewNotificationDto
    {
        public int NotificationSenderId { get; set; }
        public List<int> UserIds { get; set; }
        public bool IsReaded { get; set; }
        public NotificationType Type { get; set; }
        public bool Accepted { get; set; }
    }
}
