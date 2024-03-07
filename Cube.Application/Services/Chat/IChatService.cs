using Cube.Core.Models;
using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;
using Cube.Core.Models.Chat;

namespace Cube.Application.Services.Chat
{
    public interface IChatService
    {
        public Task<Response<List<ChatModel>, GetAllChatsResult>> GetAllUsersChats(FindUserDto dto);
        public Task<Response<ChatEntity, GetChatResult>> GetChatById(FindChatDto dto);
        public Task<Response<ChatModel, CreateChatResult>> CreateChat(NewChatDto dto);
        public Task<Response<bool, DeleteChatResult>> DeleteChat(DeleteChatDto dto);
        public Task<Response<ChatModel, UpdateChatResult>> UpdateChat(UpdateChatDto dto);
    }
}
