using Cube.Core.Models;
using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;

namespace Cube.Application.Services.Chat
{
    public interface IChatService
    {
        public Task<Response<List<ChatModel>, GetAllChatsResult>> GetAll(FindUserDto dto);
        public Task<Response<ChatModel, GetChatResult>> GetChatById(FindChatDto dto);
        public Task<Response<ChatModel, CreateChatResult>> CreateChat(NewChatDto dto);
        public Task<Response<ChatModel, DeleteChatResult>> DeleteChat(DeleteChatDto dto);
        public Task<Response<ChatModel, UpdateChatResult>> UpdateChat(UpdateChatDto dto);
    }
}
