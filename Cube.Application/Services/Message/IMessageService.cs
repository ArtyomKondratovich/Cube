using Cube.Application.Services.Message.Dto;
using Cube.Core.Models;

namespace Cube.Application.Services.Message
{
    public interface IMessageService
    {
        public Task<Response<MessageModel>> GetMessageById(FindMessageDto dto);
        public Task<Response<MessageModel>> SendMessage(NewMessageDto dto);
        public Task<Response<MessageModel>> DeleteMessage(DeleteMessageDto dto);
        public Task<Response<MessageModel>> UpdateMessage(UpdateMessageDto dto);
    }
}
