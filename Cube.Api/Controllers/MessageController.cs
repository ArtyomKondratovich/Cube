﻿using Cube.Application.Services;
using Cube.Application.Services.Message;
using Cube.Application.Services.Message.Dto;
using Cube.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Cube.Core.Models.Messages;

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
        public async Task<Response<MessageModel, SendMessageResult>> SendMessage([FromBody] NewMessageDto newMessage)
        {
            return await _service.SendMessage(newMessage);
        }

        [HttpPost]
        [Route("update")]
        public async Task<Response<MessageModel, UpdateMessageResult>> UpdateMessage([FromBody] UpdateMessageDto dto)
        {
            return await _service.UpdateMessage(dto);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<Response<MessageEntity, DeleteMessageResult>> DeleteMessage([FromBody] DeleteMessageDto dto)
        {
            return await _service.DeleteMessage(dto);
        }
    }
}
