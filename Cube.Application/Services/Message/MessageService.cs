using Cube.Application.Services.Message.Dto;
using Cube.Core.Models;
using Cube.EntityFramework.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Application.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IRepositoryWrapper _repository;

        public MessageService(IRepositoryWrapper repository) 
        {
            _repository = repository;
        }

        public async Task<Response<MessageModel>> DeleteMessage(DeleteMessageDto dto)
        {
            var response = new Response<MessageModel> 
            {
                ActionResult = new BadRequestResult(),
                Messages = new()
            };

            var message = await _repository.MessageRepository.GetMessageById(dto.Id);

            if (message != null &&
                await _repository.MessageRepository.DeleteMessage(message) != null)
            {
                response.Value = message;
                response.ActionResult = new OkResult();
            }
            else
            {
                response.Messages.Add($"Message with id = {dto.Id} not found");
            }

            return response;
        }

        public async Task<Response<MessageModel>> GetMessageById(FindMessageDto dto)
        {
            var response = new Response<MessageModel> 
            {
                ActionResult = new BadRequestResult(),
                Messages = new()
            };

            var message = await _repository.MessageRepository.GetMessageById(dto.Id);

            if (message == null)
            {
                response.Messages.Add($"Message with id = {dto.Id} not found");
            }
            else 
            {
                response.Value = message;
            }

            return response;
        }

        public async Task<Response<MessageModel>> SendMessage(NewMessageDto dto)
        {
            var response = new Response<MessageModel> 
            {
                ActionResult = new BadRequestResult(),
                Messages = new()
            };

            var user = await _repository.UserRepository.GetUserById(dto.SenderId);
            var chat = await _repository.ChatRepository.GetChatById(dto.ChatId);

            if (chat == null)
            {
                response.Messages.Add($"Chat with id = {dto.ChatId} not found");
            }

            if (user == null)
            {
                response.Messages.Add($"User with id = {dto.SenderId} not found");
            }

            if (string.IsNullOrWhiteSpace(dto.Message))
            {
                response.Messages.Add("NotOrEmpty message");
            }

            if (!response.Messages.Any())
            {
                var message = new MessageModel 
                {
                    Message = dto.Message,
                    CreatedDate = DateTime.UtcNow,
                    Sender = user,
                    Chat = chat
                };

                var value = await _repository.MessageRepository.SendMessage(message);

                if (value != null)
                {
                    response.ActionResult = new OkResult();
                    response.Value = value;
                }

            }

            return response;
        }

        public async Task<Response<MessageModel>> UpdateMessage(UpdateMessageDto dto)
        {
            var response = new Response<MessageModel> 
            {
                ActionResult = new BadRequestResult(),
                Messages = new()
            };

            var message = await _repository.MessageRepository.GetMessageById(dto.Id);

            if (message == null)
            {
                response.Messages.Add($"Message with id = {dto.Id} not found");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(dto.NewMessage) && 
                    await _repository.MessageRepository.DeleteMessage(message) != null)
                {
                   response.Messages.Add($"Deleted message with id = {message.Id}");
                   response.ActionResult = new OkResult();
                   return response;
                }
                
                if (!message.Message.Equals(dto.NewMessage))
                {
                    message.Message = dto.NewMessage;
                    message.UpdateDate = DateTime.UtcNow;
                    var result = await _repository.MessageRepository.UpdateMessage(message);

                    if (result != null)
                    {
                        response.Value = result;
                    }
                }

                response.ActionResult = new OkResult();
            }

            return response;
        }
    }
}
