using Cube.Services.Services.Message.Dto;
using Cube.Core.Models.Message;

namespace Cube.Services.Services.Message
{
    public interface IMessageService
    {
        public Task<Response<MessageModel, GetMessageResult>> GetMessageById(FindMessageDto dto);
        public Task<Response<MessageModel, SendMessageResult>> SendMessage(NewMessageDto dto);
        public Task<Response<bool, DeleteMessageResult>> DeleteMessage(DeleteMessageDto dto);
        public Task<Response<MessageModel, UpdateMessageResult>> UpdateMessage(UpdateMessageDto dto);
        public Task<Response<List<MessageModel>, GetChatMessagesResult>> GetChatMessages(ChatMessagesDto dto);
    }
}
