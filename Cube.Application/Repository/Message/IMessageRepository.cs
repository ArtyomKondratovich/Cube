using Cube.Application.Repository.Message.Dto;

namespace Cube.Application.Repository.Message
{
    public interface IMessageRepository
    {
        Task<Response> SendMessage(NewMessageDto dto);
        Task<Response> UpdateMessage(UpdateMessageDto dto);
        Task<Response> DeleteMessage(DeleteMessageDto dto);
    }
}
