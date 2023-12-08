using Cube.Server.Models;
using Cube.Server.Models.Dto;
using Cube.Server.Models.ResultObjects;

namespace Cube.Server.Repository.Interfaces
{
    public interface IMessageRepository
    {
        Result SendMessage(NewMessageDto dto);

        Result UpdateMessage(UpdateMessageDto dto);

        Result DeleteMessage(DeleteMessageDto dto);
    }
}
