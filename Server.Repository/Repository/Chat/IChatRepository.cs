using Cube.Core.Models;

namespace Cube.EntityFramework.Repository.Chat
{
    public interface IChatRepository
    {
        // ??
        List<ChatModel> GetAllUsersChats(int id);
        Task<bool> CreateChat(ChatModel model);
        Task<bool> DeleteChat(ChatModel model);
    }
}