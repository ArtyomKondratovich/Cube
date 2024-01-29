using Cube.Core.Models.User;

namespace Cube.Core.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public UserModel Sender { get; set; }
        public ChatModel Chat { get; set; }
    }
}