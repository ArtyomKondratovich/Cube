using Cube.Application.Repository;
using Cube.Application.Repository.User.Dto;
using Cube.Application.Repository.Chat.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Web.Api.Controllers
{
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IRepositoryWrapper _wrapper;

        public ChatController(IRepositoryWrapper repository) 
        {
            _wrapper = repository;
        }

        [HttpPost]
        [Route("GetAll")]
        public async Task<Response> GetAllUsersChats([FromBody] FindUserDto dto)
        {
            var result = await _wrapper.Chat.GetAllUsersChats(dto);

            return result;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<Response> CreateChat([FromBody] NewChatDto dto)
        {
            var result = await _wrapper.Chat.CreateChat(dto);

            return result;
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<Response> DeleteChat([FromBody] DeleteChatDto dto)
        {
            var result = await _wrapper.Chat.DeleteChat(dto);

            return result;
        }
    }
}
