using Cube.Application.Repository.Message.Dto;

namespace Cube.Application.Repository.Message
{
    public interface IMessageRepository
    {
        Task<Result> SendMessage(NewMessageDto dto);
        Task<Result> UpdateMessage(UpdateMessageDto dto);
        Task<Result> DeleteMessage(DeleteMessageDto dto);
    }
}
