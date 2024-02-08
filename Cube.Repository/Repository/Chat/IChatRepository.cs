using Cube.Core.Models;

namespace Cube.EntityFramework.Repository.Chat
{
    public interface IChatRepository
    {
        //
        Task<ChatEntity?> GetChatByIdAsync(int id);
        ChatEntity? GetChatById(int id);
        List<ChatEntity> GetAllUsersChats(int id);
        Task<ChatEntity?> CreateChat(ChatEntity entity);
        Task<ChatEntity?> DeleteChat(ChatEntity entity);
        Task<ChatEntity?> UpdateChat(ChatEntity entity);
        ICollection<ChatEntity> GetEntitiesByIds(ICollection<int> ids);
    }
}