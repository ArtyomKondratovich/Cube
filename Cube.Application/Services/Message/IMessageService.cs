using Cube.Application.Services.Message.Dto;
using Cube.Core.Models;
using Cube.Core.Models.Messages;

namespace Cube.Application.Services.Message
{
    public interface IMessageService
    {
        public Task<Response<MessageEntity, GetMessageResult>> GetMessageById(FindMessageDto dto);
        public Task<Response<MessageModel, SendMessageResult>> SendMessage(NewMessageDto dto);
        public Task<Response<MessageEntity, DeleteMessageResult>> DeleteMessage(DeleteMessageDto dto);
        public Task<Response<MessageModel, UpdateMessageResult>> UpdateMessage(UpdateMessageDto dto);
    }
}
