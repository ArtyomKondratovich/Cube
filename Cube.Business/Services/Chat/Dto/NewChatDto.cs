using Cube.Domain.Enums;
using Cube.Domain.Models.User;
using System.Text.Json.Serialization;

namespace Cube.Business.Services.Chat.Dto
{
    public class NewChatDto
    {
        public string Title { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ChatType Type { get; set; }
        public ICollection<int> PatricipantsIds { get; set; }
    }
}
