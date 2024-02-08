using Cube.Core.Utilities;
using System.Text.Json.Serialization;

namespace Cube.Application.Services.Chat.Dto
{
    public class DeleteChatDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ChatDeletionType DeletionType { get; set; }
    }
}
