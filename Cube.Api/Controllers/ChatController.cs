using Cube.Application.Services;
using Cube.Application.Services.Chat;
using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;
using Cube.Core.Models;
using Cube.Core.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Web.Api.Controllers
{
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _service;

        public ChatController(IChatService service) 
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        [Route("GetAll")]
        public async Task<Response<List<ChatEntity>, GetAllChatsResult>> GetAllUsersChats([FromBody] FindUserDto dto)
        {
            return await _service.GetAll(dto);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<Response<ChatEntity, CreateChatResult>> CreateChat([FromBody] NewChatDto dto)
        {
            return await _service.CreateChat(dto);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<Response<ChatEntity, DeleteChatResult>> DeleteChat([FromBody] DeleteChatDto dto)
        {
            return await _service.DeleteChat(dto);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<Response<ChatEntity, UpdateChatResult>> UpdateChat([FromBody] UpdateChatDto dto)
        {
            return await _service.UpdateChat(dto);
        }

        [HttpPost]
        [Route("GetChat")]
        public async Task<Response<ChatEntity, GetChatResult>> GetChat([FromBody] FindChatDto dto)
        {
            return await _service.GetChatById(dto);
        }
    }
}
