using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.Message.Dto;
using Cube.Application.Utilities;
using Cube.Core.Models;
using Cube.Core.Models.Messages;
using Cube.EntityFramework.Repository;
using System.Runtime.InteropServices;

namespace Cube.Application.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IRepositoryWrapper _repository;

        public MessageService(IRepositoryWrapper repository) 
        {
            _repository = repository;
        }

        public async Task<Response<bool, DeleteMessageResult>> DeleteMessage(DeleteMessageDto dto)
        {
            var response = new Response<bool, DeleteMessageResult>();

            var message = await _repository.MessageRepository.GetMessageById(dto.Id);

            if (message == null)
            {
                response.ResponseResult = DeleteMessageResult.MessageNotFound;
                response.Messages.Add($"Message with id = {dto.Id} not found");
            }
            else if (await _repository.MessageRepository.DeleteMessage(message) != null)
            {
                response.ResponseResult = DeleteMessageResult.Success;
                response.Value = true;
            }

            return response;
        }

        public async Task<Response<List<MessageModel>, GetChatMessagesResult>> GetChatMessages(FindChatMessagesDto dto)
        {
            var response = new Response<List<MessageModel>, GetChatMessagesResult>();

            var chat = await _repository.ChatRepository.GetChatByIdAsync(dto.Id);

            if (chat == null) 
            {
                response.ResponseResult = GetChatMessagesResult.ChatNotFound;
                return response;
            }

            var messages = await _repository.MessageRepository.GetChatMessagesAsync(dto.Id);
            
            if (messages != null)
            {
                response.ResponseResult = GetChatMessagesResult.Success;

                foreach (var message in messages) 
                {
                    message.CreatedDate = message.CreatedDate.AddHours(dto.UsersTimezoneOffset);
                }

                response.Value = messages
                    .Select(x => MapperConfig.InitializeAutomapper().Map<MessageModel>(x))
                    .ToList();
                return response;
            }

            response.ResponseResult = GetChatMessagesResult.ServerError;
            return response;

        }

        public async Task<Response<MessageModel, GetMessageResult>> GetMessageById(FindMessageDto dto)
        {
            var response = new Response<MessageModel, GetMessageResult>();

            var message = await _repository.MessageRepository.GetMessageById(dto.Id);

            if (message == null)
            {
                response.ResponseResult = GetMessageResult.MessageNotFound;
                response.Messages.Add($"Message with id = {dto.Id} not found");
            }
            else 
            {
                response.ResponseResult = GetMessageResult.Success;
                response.Value = MapperConfig.InitializeAutomapper().Map<MessageModel>(message);
            }

            return response;
        }

        public async Task<Response<MessageModel, SendMessageResult>> SendMessage(NewMessageDto dto)
        {
            var response = new Response<MessageModel, SendMessageResult>(); 
            

            var user = await _repository.UserRepository.GetUserByIdAsync(dto.SenderId);
            var chat = await _repository.ChatRepository.GetChatByIdAsync(dto.ChatId);

            if (user == null)
            {
                response.ResponseResult = SendMessageResult.UserNotFound;
                response.Messages.Add($"User with id = {dto.SenderId} not found");
                return response;
            }

            if (chat == null)
            {
                response.ResponseResult = SendMessageResult.ChatNotFound;
                response.Messages.Add($"Chat with id = {dto.ChatId} not found");
                return response;
            }

            if (string.IsNullOrWhiteSpace(dto.Message))
            {
                response.ResponseResult = SendMessageResult.ValidationError;
                response.Messages.Add("NullOrEmpty message");
                return response;
            }

            var message = new MessageEntity 
            {
                Message = dto.Message,
                CreatedDate = DateTime.UtcNow,
                UserId = user.Id,
                ChatId = chat.Id
            };

            var value = await _repository.MessageRepository.SendMessage(message);

            if (value == null)
            {
                response.ResponseResult = SendMessageResult.DataBaseError;
            }
            else
            {
                response.ResponseResult = SendMessageResult.Success;
                response.Value = MapperConfig.InitializeAutomapper().Map<MessageModel>(value);
            }

            return response;
        }

        public async Task<Response<MessageModel, UpdateMessageResult>> UpdateMessage(UpdateMessageDto dto)
        {
            var response = new Response<MessageModel, UpdateMessageResult>();

            var message = await _repository.MessageRepository.GetMessageById(dto.Id);
            var updater = await _repository.UserRepository.GetUserByIdAsync(dto.UpdaterId);

            if (updater == null)
            {
                response.ResponseResult = UpdateMessageResult.UserNotFound;
                response.Messages.Add($"User with id = {dto.UpdaterId} not found");
                return response;
            }

            if (message == null)
            {
                response.ResponseResult = UpdateMessageResult.MessageNotFound;
                response.Messages.Add($"Message with id = {dto.Id} not found");
                return response;
            }
            
            if (string.IsNullOrWhiteSpace(dto.NewMessage))
            {
                response.ResponseResult = UpdateMessageResult.NullOrEmptyNewMessage;
                return response;
            }
            
            if (!message.Message.Equals(dto.NewMessage))
            {
                message.Message = dto.NewMessage;
                message.UpdateDate = DateTime.UtcNow;
                var result = await _repository.MessageRepository.UpdateMessage(message);

                if (result != null)
                {
                    response.Value = MapperConfig.InitializeAutomapper().Map<MessageModel>(result);
                }
            }

            response.ResponseResult = UpdateMessageResult.Success;
            return response;
        }
    }
}
