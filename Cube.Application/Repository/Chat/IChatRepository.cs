using Cube.Application.Repository.User.Dto;
using Cube.Application.Repository.Chat.Dto;

namespace Cube.Application.Repository.Chat
{
    public interface IChatRepository
    {
        Task<Response> GetAllUsersChats(FindUserDto dto);
        Task<Response> CreateChat(NewChatDto dto);
        Task<Response> DeleteChat(DeleteChatDto dto);
    }
}
