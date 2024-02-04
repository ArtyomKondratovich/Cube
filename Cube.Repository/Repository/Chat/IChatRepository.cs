using Cube.Core.Models;

namespace Cube.EntityFramework.Repository.Chat
{
    public interface IChatRepository
    {
        //
        Task<ChatModel?> GetChatById(int id);
        List<ChatModel> GetAllUsersChats(int id);
        Task<ChatModel?> CreateChat(ChatModel model);
        Task<ChatModel?> DeleteChat(ChatModel model);
        Task<ICollection<ChatModel>> GetEntitiesByIds(ICollection<int> ids);
    }
}