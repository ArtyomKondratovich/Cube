using Cube.Application.Utilities;

namespace Cube.Application.Repository.Chat.Dto
{
    public class DeleteChatDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ChatDeletionType DeletionType { get; set; }
    }
}
