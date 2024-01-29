using Cube.Core.Models.User;
using Cube.Core.Utilities;

namespace Cube.Application.Services.Chat.Dto
{
    public class NewChatDto
    {
        public string Title { get; set; }
        public UserModel? Admin { get; set; }
        public ChatType Type { get; set; }
        public ICollection<UserModel> Patricipants { get; set; }
    }
}
