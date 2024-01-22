using Cube.EntityFramework.Repository.Message.Dto;
using Cube.Core.Models;

namespace Cube.Application.Repository.Message
{
    public interface IMessageRepository
    {
        Task<MessageModel?> GetMessageById(int id);
        Task<bool> SendMessage(MessageModel model);
        Task<bool> UpdateMessage(MessageModel model);
        Task<bool> DeleteMessage(DeleteMessageDto dto);
    }
}
