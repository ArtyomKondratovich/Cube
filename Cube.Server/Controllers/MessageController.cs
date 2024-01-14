using Cube.Application.Repository;
using Cube.Application.Repository.Message.Dto;

using Microsoft.AspNetCore.Mvc;

namespace Cube.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public MessageController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("Send")]
        public async Task<Response> SendMessage([FromBody] NewMessageDto newMessage)
        {
            var result = await _repository.Message.SendMessage(newMessage);

            if (result.ReturnObject is bool value && value)
            {
                _repository.Save();
            }

            return result;
        }

        [HttpPost]
        [Route("Update")]
        public async Task<Response> UpdateMessage([FromBody] UpdateMessageDto dto)
        {
            var result = await _repository.Message.UpdateMessage(dto);

            if (result.ReturnObject is bool value && value)
            {
                _repository.Save();
            }

            return result;
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<Response> DeleteMessage([FromBody] DeleteMessageDto dto)
        {
            var result = await _repository.Message.DeleteMessage(dto);

            if (result.ReturnObject is bool value && value)
            {
                _repository.Save();
            }

            return result;
        }
    }
}
