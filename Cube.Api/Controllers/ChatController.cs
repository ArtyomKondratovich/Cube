using Cube.Application.Services;
using Cube.Application.Services.Chat;
using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;
using Cube.Core.Models;
using Cube.Core.Models.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Web.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _service;

        public ChatController(IChatService service) 
        {
            _service = service;
        }

        [HttpPost]
        [Route("GetAll")]
        public async Task<Response<List<ChatModel>, GetAllChatsResult>> GetAllUsersChats([FromBody] FindUserDto dto)
        {
            return await _service.GetAllUsersChats(dto);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<Response<ChatModel, CreateChatResult>> CreateChat([FromBody] NewChatDto dto)
        {
            return await _service.CreateChat(dto);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<Response<bool, DeleteChatResult>> DeleteChat([FromBody] DeleteChatDto dto)
        {
            return await _service.DeleteChat(dto);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<Response<ChatModel, UpdateChatResult>> UpdateChat([FromBody] UpdateChatDto dto)
        {
            return await _service.UpdateChat(dto);
        }

        [HttpPost]
        [Route("Get")]
        public async Task<Response<ChatModel, GetChatResult>> GetChat([FromBody] FindChatDto dto)
        {
            return await _service.GetChatById(dto);
        }
    }
}
