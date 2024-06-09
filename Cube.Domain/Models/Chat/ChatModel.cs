using Cube.Domain.Enums;
using Cube.Domain.Models.User;

namespace Cube.Domain.Models.Chat
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ChatType Type { get; set; }
        public List<UserModel> Users { get; set; }
    }
}
