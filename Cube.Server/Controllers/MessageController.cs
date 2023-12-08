using Cube.Server.Models;
using Cube.Server.Models.Dto;
using Cube.Server.Models.ResultObjects;
using Cube.Server.Repository.Interfaces;
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
        public Result SendMessage([FromBody] NewMessageDto newMessage)
        {
            var result = _repository.Message.SendMessage(newMessage);

            if (result.ReturnObject is bool value && value)
            {
                _repository.Save();
            }

            return result;
        }

        [HttpPost]
        public Result UpdateMessage([FromBody] UpdateMessageDto dto)
        {
            var result = _repository.Message.UpdateMessage(dto);

            if (result.ReturnObject is bool value && value)
            {
                _repository.Save();
            }

            return result;
        }

        [HttpPost]
        public Result DeleteMessage([FromBody] DeleteMessageDto dto) 
        {
            var result = _repository.Message.DeleteMessage(dto);

            if (result.ReturnObject is bool value && value)
            {
                _repository.Save();
            }

            return result;
        }
    }
}
