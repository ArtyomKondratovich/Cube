using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;
using Cube.Core.Models;
using Cube.Core.Utilities;
using Cube.EntityFramework.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Application.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IRepositoryWrapper _repository;

        public ChatService(IRepositoryWrapper repository) 
        {
            _repository = repository;
        }

        public async Task<Response<ChatModel>> CreateChat(NewChatDto dto)
        {
            var responce = new Response<ChatModel>
            {
                ActionResult = new BadRequestResult(),
                Messages = new()
            };

            if (dto != null) 
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                {
                    responce.Messages.Add("NullOrEmptyTitle");
                }
                else
                {
                    responce.ActionResult = new OkResult();
                    responce.Value = await _repository.ChatRepository.CreateChat(new ChatModel
                    {
                        Title = dto.Title,
                        ChatAdmin = dto.Admin,
                        Type = dto.ChatType,
                        Participants = dto.Patricipants
                    });
                }
            }

            return responce;
        }

        public async Task<Response<ChatModel>> DeleteChat(DeleteChatDto dto)
        {
            var responce = new Response<ChatModel>
            {
                ActionResult = new BadRequestResult()
            };

            var chat = await _repository.ChatRepository.GetChatById(dto.Id);

            if (chat != null) 
            {
                switch (dto.DeletionType)
                {
                    case ChatDeletionType.RemoveFromMessageList:
                        // Update Chat remove from particpants
                        break;
                    case ChatDeletionType.RemoveAndDeleteMessages:
                        // Update Chat remove users messages in messages
                        break;
                    case ChatDeletionType.CompleteRemoval:
                        break;
                }
            }

            return responce;
        }

        public Response<List<ChatModel>> GetAllUsersChats(FindUserDto dto)
        {
            
            var responce = new Response<List<ChatModel>>
            {
                Value = _repository.ChatRepository.GetAllUsersChats(dto.Id),
                ActionResult = new OkResult(),
                Messages = new()
            };

            if (responce.Value == null)
            {
                responce.Messages.Add("User has no chats");
                responce.ActionResult = new BadRequestResult();
            }

            return responce;
        }
    }
}
