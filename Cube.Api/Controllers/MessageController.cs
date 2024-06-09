using Microsoft.AspNetCore.Mvc;
using Cube.Business.Services.Message;
using Cube.Domain.Models.Message;
using Cube.Business.Services;
using Cube.Business.Utilities;
using Cube.Business.Services.Message.Dto;

namespace Cube.Web.Api.Controllers
{
    //[Authorize]
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
        [Route("send")]
        public async Task<Response<MessageModel, SendMessageResult>> SendMessage([FromBody] NewMessageDto dto)
        {
            return await _service.SendMessage(dto);
        }

        [HttpPost]
        [Route("update")]
        public async Task<Response<MessageModel, UpdateMessageResult>> UpdateMessage([FromBody] UpdateMessageDto dto)
        {
            return await _service.UpdateMessage(dto);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<Response<bool, DeleteMessageResult>> DeleteMessage([FromBody] DeleteMessageDto dto)
        {
            return await _service.DeleteMessage(dto);
        }

        [HttpPost]
        [Route("getChatMessages")]
        public async Task<Response<List<MessageModel>, GetChatMessagesResult>> GetChatMessages([FromBody] ChatMessagesDto dto)
        {
            return await _service.GetChatMessages(dto);
        }
    }
}
