using Cube.Application.Services.Message.Dto;
using Cube.Core.Models;

namespace Cube.Application.Services.Message
{
    public interface IMessageService
    {
        public Task<Response<MessageEntity, GetMessageResult>> GetMessageById(FindMessageDto dto);
        public Task<Response<MessageEntity, SendMessageResult>> SendMessage(NewMessageDto dto);
        public Task<Response<MessageEntity, DeleteMessageResult>> DeleteMessage(DeleteMessageDto dto);
        public Task<Response<MessageEntity, UpdateMessageResult>> UpdateMessage(UpdateMessageDto dto);
    }
}
