using Cube.Core.Models;
using Cube.Core.Utilities;

namespace Cube.Application.Services.Chat.Dto
{
    public class NewChatDto
    {
        public string Title { get; set; }
        public UserModel? Admin { get; set; }
        public ChatType ChatType { get; set; }
        public ICollection<UserModel> Patricipants { get; set; }
    }
}
