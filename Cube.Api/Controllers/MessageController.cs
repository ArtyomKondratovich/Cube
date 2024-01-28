using Cube.Application.Services;
using Cube.Application.Services.Message;
using Cube.Application.Services.Message.Dto;
using Cube.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _service;

        public MessageController(IMessageService repository)
        {
            _service = repository;
        }

        [HttpPost]
        [Route("Send")]
        public async Task<Response<MessageModel>> SendMessage([FromBody] NewMessageDto newMessage)
        {
            return await _service.SendMessage(newMessage);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<Response<MessageModel>> UpdateMessage([FromBody] UpdateMessageDto dto)
        {
            return await _service.UpdateMessage(dto);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<Response<MessageModel>> DeleteMessage([FromBody] DeleteMessageDto dto)
        {
            return await _service.DeleteMessage(dto);
        }
    }
}
