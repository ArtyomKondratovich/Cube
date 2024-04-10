using Cube.Core.Models;
using Cube.Core.Models.Chat;

namespace Cube.EntityFramework.Repository.Chat
{
    public interface IChatRepository
    {
        Task<ChatEntity?> GetChatByIdAsync(int id);
        ChatEntity? GetChatById(int id);
        Task<List<ChatEntity>> GetAllUsersChatsAsync(int id);
        Task<ChatEntity> CreateChat(ChatEntity entity);
        Task<bool> DeleteChat(int id);
        Task<ChatEntity?> UpdateChat(ChatEntity entity);
        ICollection<ChatEntity> GetEntitiesByIds(ICollection<int> ids);
    }
}