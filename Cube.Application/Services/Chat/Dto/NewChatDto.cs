using Cube.Core.Models.User;
using Cube.Core.Utilities;
using System.Text.Json.Serialization;

namespace Cube.Application.Services.Chat.Dto
{
    public class NewChatDto
    {
        public string Title { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ChatType Type { get; set; }
        public ICollection<int> PatricipantsIds { get; set; }
    }
}
