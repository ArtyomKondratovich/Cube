namespace Cube.Server.Models
{
    public class CorrespondenceModel
    {
        public List<MessageModel> UsersMessages { get; set; }
        public List<MessageModel> FriendMessages { get; set; }
    }
}
