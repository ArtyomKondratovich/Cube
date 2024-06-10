using Cube.Business.Services.Message.Dto;
using Cube.Business.Utilities;
using Cube.Domain.Models;
using Cube.Domain.Models.Message;
using Cube.DataAccess.Repositories;
using System.Linq.Expressions;
using AutoMapper;

namespace Cube.Business.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly int _maxMessagesRecieve;
        private readonly Mapper _mapper;

        public MessageService(IRepositoryWrapper repository, 
            int maxTake,
            Mapper mapper) 
        {
            _repository = repository;
            _maxMessagesRecieve = maxTake;
            _mapper = mapper;
        }

        public async Task<Response<bool, DeleteMessageResult>> DeleteMessage(DeleteMessageDto dto)
        {
            var response = new Response<bool, DeleteMessageResult>();

            var message = await _repository.MessageRepository.GetByIdAsync(dto.MessageId);

            if (message == null)
            {
                response.ResponseResult = DeleteMessageResult.MessageNotFound;
                response.Messages.Add($"Message with id = {dto.MessageId} not found");
            }
            else if (await _repository.MessageRepository.DeleteAsync(message))
            {
                response.ResponseResult = DeleteMessageResult.Success;
                response.Value = true;
            }

            return response;
        }

        public async Task<Response<List<MessageModel>, GetChatMessagesResult>> GetChatMessages(ChatMessagesDto dto)
        {
            var response = new Response<List<MessageModel>, GetChatMessagesResult>();

            var chat = await _repository.ChatRepository.GetByIdAsync(dto.ChatId);

            if (chat == null) 
            {
                response.ResponseResult = GetChatMessagesResult.ChatNotFound;
                return response;
            }

            if (dto.Take > _maxMessagesRecieve)
            {
                response.ResponseResult = GetChatMessagesResult.ServerError;
                response.Messages.Add($"The maximum number ({_maxMessagesRecieve}) of messages received has been exceeded, you tried to recive {dto.Take} messages");
                return response;
            }

            if (dto.Skip < 0)
            {
                response.ResponseResult = GetChatMessagesResult.ServerError;
                response.Messages.Add($"Skip number should be equals or greater than 0, you tried skip {dto.Skip} messages");
                return response;
            }

            Expression<Func<MessageEntity, bool>> filter = (message) => message.ChatId == dto.ChatId;

            var messages = await _repository.MessageRepository.GetChatMessagesAsync(dto.ChatId, dto.Take, dto.Skip);
            
            if (messages != null)
            {
                response.ResponseResult = GetChatMessagesResult.Success;
                response.Value = messages
                    .Select(x => _mapper.Map<MessageModel>(x))
                    .ToList();

                return response;
            }

            response.ResponseResult = GetChatMessagesResult.ServerError;
            return response;
        }

        public async Task<Response<MessageModel, GetMessageResult>> GetMessageById(FindMessageDto dto)
        {
            var response = new Response<MessageModel, GetMessageResult>();

            var message = await _repository.MessageRepository.GetByIdAsync(dto.MessageId);

            if (message == null)
            {
                response.ResponseResult = GetMessageResult.MessageNotFound;
                response.Messages.Add($"Message with id = {dto.MessageId} not found");
            }
            else 
            {
                response.ResponseResult = GetMessageResult.Success;
                response.Value = _mapper.Map<MessageModel>(message);
            }

            return response;
        }

        public async Task<Response<MessageModel, SendMessageResult>> SendMessageAsync(NewMessageDto dto)
        {
            var response = new Response<MessageModel, SendMessageResult>(); 
            

            var user = await _repository.UserRepository.GetByIdAsync(dto.SenderId);
            var chat = await _repository.ChatRepository.GetByIdAsync(dto.ChatId);

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

            var result = await _repository.MessageRepository.CreateAsync(message);

            if (result == null)
            {
                response.ResponseResult = SendMessageResult.DataBaseError;
            }
            else
            {
                response.ResponseResult = SendMessageResult.Success;
                result.CreatedDate = result.CreatedDate.AddHours(dto.TimeZoneOffset);
                response.Value = _mapper.Map<MessageModel>(result);
            }

            return response;
        }

        public async Task<Response<MessageModel, UpdateMessageResult>> UpdateMessage(UpdateMessageDto dto)
        {
            var response = new Response<MessageModel, UpdateMessageResult>();

            var message = await _repository.MessageRepository.GetByIdAsync(dto.MessageId);
            var updater = await _repository.UserRepository.GetByIdAsync(dto.UpdaterId);

            if (updater == null)
            {
                response.ResponseResult = UpdateMessageResult.UserNotFound;
                response.Messages.Add($"User with id = {dto.UpdaterId} not found");
                return response;
            }

            if (message == null)
            {
                response.ResponseResult = UpdateMessageResult.MessageNotFound;
                response.Messages.Add($"Message with id = {dto.MessageId} not found");
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
                var result = await _repository.MessageRepository.UpdateAsync(message);

                if (result != null)
                {
                    response.Value = _mapper.Map<MessageModel>(result);
                }
            }

            response.ResponseResult = UpdateMessageResult.Success;
            return response;
        }
    }
}
