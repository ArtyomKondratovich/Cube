using Cube.Core.Models;
using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;

namespace Cube.Application.Services.Chat
{
    public interface IChatService
    {
        public Task<Response<List<ChatEntity>, GetAllChatsResult>> GetAll(FindUserDto dto);
        public Task<Response<ChatEntity, GetChatResult>> GetChatById(FindChatDto dto);
        public Task<Response<ChatEntity, CreateChatResult>> CreateChat(NewChatDto dto);
        public Task<Response<ChatEntity, DeleteChatResult>> DeleteChat(DeleteChatDto dto);
        public Task<Response<ChatEntity, UpdateChatResult>> UpdateChat(UpdateChatDto dto);
    }
}
