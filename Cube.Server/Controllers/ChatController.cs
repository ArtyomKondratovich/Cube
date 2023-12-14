using Cube.Application.Repository;
using Cube.Application.Repository.User.Dto;
using Cube.Application.Repository.Chat.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Web.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;

        public ChatController(IRepositoryWrapper repository) 
        {
            _wrapper = repository;
        }

        [HttpPost]
        public async Task<Response> GetAllUsersChats([FromBody] FindUserDto dto)
        {
            var result = await _wrapper.Chat.GetAllUsersChats(dto);

            return result;
        }

        [HttpPost]
        public async Task<Response> CreateChat([FromBody] NewChatDto dto)
        {
            var result = await _wrapper.Chat.CreateChat(dto);

            return result;
        }

        [HttpPost]
        public async Task<Response> DeleteChat([FromBody] DeleteChatDto dto)
        {
            var result = await _wrapper.Chat.DeleteChat(dto);

            return result;
        }
    }
}
