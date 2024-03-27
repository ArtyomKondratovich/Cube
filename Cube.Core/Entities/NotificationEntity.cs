using Cube.Core.Enums;

namespace Cube.Core.Entities
{
    public class NotificationEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NotificationTargetId { get; set; }
        public bool IsReaded { get; set; }
        public NotificationType Type { get; set; }
    }
}
