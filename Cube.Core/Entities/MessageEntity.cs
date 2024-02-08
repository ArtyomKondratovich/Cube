using Cube.Core.Models.User;

namespace Cube.Core.Models
{
    public class MessageEntity
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public UserEntity Sender { get; set; }
        public ChatEntity Chat { get; set; }
    }
}