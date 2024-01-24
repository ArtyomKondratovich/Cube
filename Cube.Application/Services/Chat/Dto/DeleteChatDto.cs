using Cube.Core.Utilities;

namespace Cube.Application.Services.Chat.Dto
{
    public class DeleteChatDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ChatDeletionType DeletionType { get; set; }
    }
}
