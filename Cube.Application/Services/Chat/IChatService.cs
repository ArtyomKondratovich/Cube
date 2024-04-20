using Cube.Application.Services.Chat.Dto;
using Cube.Core.Models.Chat;

namespace Cube.Application.Services.Chat
{
    public interface IChatService
    {
        public Task<Response<List<ChatModel>, GetAllChatsResult>> GetAllUserChats(FindUserChatsDto dto);
        public Task<Response<ChatModel, GetChatResult>> GetChatById(FindChatDto dto);
        public Task<Response<ChatModel, CreateChatResult>> CreateChat(NewChatDto dto);
        public Task<Response<bool, DeleteChatResult>> DeleteChat(DeleteChatDto dto);
        public Task<Response<ChatModel, UpdateChatResult>> UpdateChat(UpdateChatDto dto);
    }
}
