using Cube.Application.Repository;
using Cube.Application.Repository.Message.Dto;

using Microsoft.AspNetCore.Mvc;

namespace Cube.Server.Controllers
{
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public MessageController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<Result> SendMessage([FromBody] NewMessageDto newMessage)
        {
            var result = await _repository.Message.SendMessage(newMessage);

            if (result.ReturnObject is bool value && value)
            {
                _repository.Save();
            }

            return result;
        }

        [HttpPost]
        public async Task<Result> UpdateMessage([FromBody] UpdateMessageDto dto)
        {
            var result = await _repository.Message.UpdateMessage(dto);

            if (result.ReturnObject is bool value && value)
            {
                _repository.Save();
            }

            return result;
        }

        [HttpPost]
        public async Task<Result> DeleteMessage([FromBody] DeleteMessageDto dto)
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
