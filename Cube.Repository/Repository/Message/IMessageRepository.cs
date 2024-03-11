using Cube.Core.Models;

namespace Cube.EntityFramework.Repository.Message
{
    public interface IMessageRepository
    {
        Task<MessageEntity?> GetMessageById(int id);
        Task<MessageEntity?> SendMessage(MessageEntity model);
        Task<MessageEntity?> UpdateMessage(MessageEntity model);
        Task<MessageEntity?> DeleteMessage(MessageEntity model);
        Task<List<MessageEntity>> GetChatMessagesAsync(int chatId);
    }
}
