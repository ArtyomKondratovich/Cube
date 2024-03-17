using Cube.Core.Enums;

namespace Cube.Core.Models.Chat
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ChatType Type { get; set; }
        public List<int> Users { get; set; }
    }
}
