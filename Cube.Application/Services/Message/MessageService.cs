using Cube.Application.Services.Message.Dto;
using Cube.Core.Models;
using Cube.EntityFramework.Repository;

namespace Cube.Application.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IRepositoryWrapper _repository;

        public MessageService(IRepositoryWrapper repository) 
        {
            _repository = repository;
        }

        public async Task<Response<MessageEntity, DeleteMessageResult>> DeleteMessage(DeleteMessageDto dto)
        {
            var response = new Response<MessageEntity, DeleteMessageResult>();

            var message = await _repository.MessageRepository.GetMessageById(dto.Id);

            if (message == null)
            {
                response.ResponseResult = DeleteMessageResult.MessageNotFound;
                response.Messages.Add($"Message with id = {dto.Id} not found");
            }
            else if (await _repository.MessageRepository.DeleteMessage(message) != null)
            {
                response.ResponseResult = DeleteMessageResult.Success;
                response.Value = message;
            }

            return response;
        }

        public async Task<Response<MessageEntity, GetMessageResult>> GetMessageById(FindMessageDto dto)
        {
            var response = new Response<MessageEntity, GetMessageResult>();

            var message = await _repository.MessageRepository.GetMessageById(dto.Id);

            if (message == null)
            {
                response.ResponseResult = GetMessageResult.MessageNotFound;
                response.Messages.Add($"Message with id = {dto.Id} not found");
            }
            else 
            {
                response.ResponseResult = GetMessageResult.Success;
                response.Value = message;
            }

            return response;
        }

        public async Task<Response<MessageEntity, SendMessageResult>> SendMessage(NewMessageDto dto)
        {
            var response = new Response<MessageEntity, SendMessageResult>(); 
            

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
                Sender = user,
                Chat = chat
            };

            var value = await _repository.MessageRepository.SendMessage(message);

            if (value == null)
            {
                response.ResponseResult = SendMessageResult.DataBaseError;
            }
            else
            {
                response.ResponseResult = SendMessageResult.Success;
                response.Value = value;
            }

            return response;
        }

        public async Task<Response<MessageEntity, UpdateMessageResult>> UpdateMessage(UpdateMessageDto dto)
        {
            var response = new Response<MessageEntity, UpdateMessageResult>();

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
                    response.Value = result;
                }
            }

            response.ResponseResult = UpdateMessageResult.Success;
            return response;
        }
    }
}
