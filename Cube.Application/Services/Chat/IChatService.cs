using Cube.Core.Models;
using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;

namespace Cube.Application.Services.Chat
{
    public interface IChatService
    {
        public Response<List<ChatModel>> GetAllUsersChats(FindUserDto dto);
        public Task<Response<ChatModel>> CreateChat(NewChatDto dto);
        public Task<Response<ChatModel>> DeleteChat(DeleteChatDto dto);
    }
}
