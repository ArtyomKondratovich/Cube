using Cube.Core.Utilities;

namespace Cube.Core.Models.Chat
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ChatType Type { get; set; }
    }
}
