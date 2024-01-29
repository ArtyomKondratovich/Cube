using Cube.Core.Models.User;
using Cube.Core.Utilities;

namespace Cube.Core.Models
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public UserModel? ChatAdmin { get; set; }
        public ChatType Type { get; set; }
        public ICollection<UserModel> Participants { get; set; }
        public ICollection<MessageModel> Messages { get; set; }
    }
}