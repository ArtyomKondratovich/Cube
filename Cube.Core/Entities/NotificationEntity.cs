using Cube.Core.Enums;
using System.Text.Json.Serialization;

namespace Cube.Core.Entities
{
    public class NotificationEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NotificationSenderId { get; set; }
        public bool IsReaded { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationType Type { get; set; }
    }
}
