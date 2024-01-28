using Cube.Application.Services;
using Cube.Application.Services.Chat;
using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;
using Cube.Core.Models;
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
        [Route("GetAll")]
        public Response<List<ChatModel>> GetAllUsersChats([FromBody] FindUserDto dto)
        {
            return _service.GetAllUsersChats(dto);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<Response<ChatModel>> CreateChat([FromBody] NewChatDto dto)
        {
            return await _service.CreateChat(dto);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<Response<ChatModel>> DeleteChat([FromBody] DeleteChatDto dto)
        {
            return await _service.DeleteChat(dto);
        }
    }
}
