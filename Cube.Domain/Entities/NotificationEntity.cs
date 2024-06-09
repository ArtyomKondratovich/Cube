using Cube.Domain.Enums;
using System.Text.Json.Serialization;

namespace Cube.Domain.Entities
{
    public class NotificationEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NotificationSenderId { get; set; }
        public bool IsReaded { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationType Type { get; set; }
        public bool Accepted { get; set; }
    }
}
