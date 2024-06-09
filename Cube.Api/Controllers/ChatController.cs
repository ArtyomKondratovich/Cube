using Microsoft.AspNetCore.Mvc;
using Cube.Business.Services.Chat;
using Cube.Business.Services;
using Cube.Domain.Models.Chat;
using Cube.Business.Services.Chat.Dto;
using Cube.Business.Utilities;

namespace Cube.Api.Controllers
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
        [Route("getUserChats")]
        public async Task<Response<List<ChatModel>, GetAllChatsResult>> GetAllUserChats([FromBody] FindUserChatsDto dto)
        {
            return await _service.GetAllUserChats(dto);
        }

        [HttpPost]
        [Route("create")]
        public async Task<Response<ChatModel, CreateChatResult>> CreateChat([FromBody] NewChatDto dto)
        {
            return await _service.CreateChat(dto);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<Response<bool, DeleteChatResult>> DeleteChat([FromBody] DeleteChatDto dto)
        {
            return await _service.DeleteChat(dto);
        }

        [HttpPost]
        [Route("update")]
        public async Task<Response<ChatModel, UpdateChatResult>> UpdateChat([FromBody] UpdateChatDto dto)
        {
            return await _service.UpdateChat(dto);
        }

        [HttpPost]
        [Route("getChat")]
        public async Task<Response<ChatModel, GetChatResult>> GetChat([FromBody] FindChatDto dto)
        {
            return await _service.GetChatById(dto);
        }
    }
}
