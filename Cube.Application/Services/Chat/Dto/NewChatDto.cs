using Cube.Core.Models.User;
using Cube.Core.Utilities;

namespace Cube.Application.Services.Chat.Dto
{
    public class NewChatDto
    {
        public string Title { get; set; }
        public int? AdminId { get; set; }
        public ChatType Type { get; set; }
        public ICollection<int> PatricipantsIds { get; set; }
    }
}
