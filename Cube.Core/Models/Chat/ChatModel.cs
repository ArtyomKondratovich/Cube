using Cube.Core.Enums;
using Cube.Core.Models.User;

namespace Cube.Core.Models.Chat
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ChatType Type { get; set; }
        public List<UserModel> Users { get; set; }
    }
}
