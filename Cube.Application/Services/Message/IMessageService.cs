using Cube.Application.Services.Message.Dto;
using Cube.Core.Models;

namespace Cube.Application.Services.Message
{
    public interface IMessageService
    {
        public Task<Response<MessageModel, GetMessageResult>> GetMessageById(FindMessageDto dto);
        public Task<Response<MessageModel, SendMessageResult>> SendMessage(NewMessageDto dto);
        public Task<Response<MessageModel, DeleteMessageResult>> DeleteMessage(DeleteMessageDto dto);
        public Task<Response<MessageModel, UpdateMessageResult>> UpdateMessage(UpdateMessageDto dto);
    }
}
