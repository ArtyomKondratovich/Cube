using Cube.Core.Models.User;
using Cube.Core.Utilities;

namespace Cube.Core.Models
{
    public class ChatEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public UserEntity? Admin { get; set; }
        public ChatType Type { get; set; }
        public ICollection<UserEntity> Participants { get; set; }
        public ICollection<MessageEntity> Messages { get; set; }
    }
}