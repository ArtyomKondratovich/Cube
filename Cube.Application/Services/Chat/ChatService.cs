using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;
using Cube.Application.Utilities;
using Cube.Application.Services;
using Cube.Core.Models;
using Cube.Core.Models.User;
using Cube.Core.Utilities;
using Cube.EntityFramework.Repository;

namespace Cube.Application.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IRepositoryWrapper _repository;

        public ChatService(IRepositoryWrapper repository) 
        {
            _repository = repository;
        }

        public async Task<Response<ChatEntity, CreateChatResult>> CreateChat(NewChatDto dto)
        {
            var responce = new Response<ChatEntity, CreateChatResult>();
            

            if (dto != null) 
            {
                // checking chat name 
                if (string.IsNullOrWhiteSpace(dto.Title))
                {
                    responce.Messages.Add("NullOrEmptyTitle");
                    responce.ResponseResult = CreateChatResult.ValidationError;
                    return responce;
                }

                // checking the existence of participants
                if (!dto.PatricipantsIds.IsEntitiesExist<UserEntity>(_repository))
                {
                    responce.ResponseResult = CreateChatResult.ParticipantsNotFound; 
                    return responce;
                }

                // checking chat settings
                var isProperlyCofigured = dto.Type switch
                {
                    ChatType.Private => dto.AdminId == null,
                    ChatType.Group => dto.AdminId != null,
                    _ => false
                };

                if (!isProperlyCofigured)
                {
                    responce.ResponseResult = CreateChatResult.WrongChatSettings;
                    return responce;
                }

                var chat = MapperConfig.InitializeAutomapper(_repository).Map<ChatEntity>(dto);
                chat.Participants = dto.PatricipantsIds
                    .Select(id => _repository.UserRepository.GetUserById(id))
                    .Where(user => user != null)
                    .ToList();
                chat.Admin = _repository.UserRepository.GetUserById(dto.AdminId??default);

                if (await _repository.ChatRepository.CreateChat(chat) != null)
                {
                    responce.ResponseResult = CreateChatResult.Success;
                    responce.Value = chat;
                    return responce;
                }
            }

            return responce;
        }

        public async Task<Response<ChatEntity, DeleteChatResult>> DeleteChat(DeleteChatDto dto)
        {
            var response = new Response<ChatEntity, DeleteChatResult>();

            var chat = await _repository.ChatRepository.GetChatByIdAsync(dto.Id);
            var user = await _repository.UserRepository.GetUserByIdAsync(dto.Id);

            if (user == null)
            {
                response.ResponseResult = DeleteChatResult.UserNotFound;
                return response;
            }

            if (chat == null)
            {
                response.ResponseResult = DeleteChatResult.ChatNotFound;
                return response;
            }

            var currentParticipants = chat.Participants.Select(x => x.Id).ToList();
           
            switch (dto.DeletionType)
            {
                case ChatDeletionType.RemoveFromMessageList:
                    // Update Chat remove from particpants
                    currentParticipants.Remove(user.Id);
                    break;
                case ChatDeletionType.RemoveAndDeleteMessages:
                    // Update Chat remove users messages in messages
                    currentParticipants.Remove(user.Id);

                    var usersMessages = chat.Messages.Where(x => x.Sender.Id == user.Id).ToList();
                    foreach (var message in  usersMessages)
                    {
                        await _repository.MessageRepository.DeleteMessage(message);
                    }
                    break;
                case ChatDeletionType.CompleteRemoval when chat.Type == ChatType.Group:
                    if (chat.Admin != null && chat.Admin.Id == user.Id)
                    {
                        currentParticipants = new List<int>();
                        
                        if (chat != null)
                        {
                            foreach (var message in chat.Messages)
                            {
                                await _repository.MessageRepository.DeleteMessage(message);
                            }
                        }
                    }
                    break;
                default:
                    chat.Participants = currentParticipants
                        .Select(x => _repository.UserRepository.GetUserById(x))
                        .ToList();
                    chat = await _repository.ChatRepository.UpdateChat(chat);
                    if (chat != null) 
                    {
                        response.ResponseResult = DeleteChatResult.Success;
                        response.Value = chat;
                    }
                    break;
            }
            
            return response;
        }

        public async Task<Response<List<ChatEntity>, GetAllChatsResult>> GetAll(FindUserDto dto)
        {
            var response = new Response<List<ChatEntity>, GetAllChatsResult>();

            if (await _repository.UserRepository.GetUserByIdAsync(dto.Id) == null)
            {
                response.ResponseResult = GetAllChatsResult.UserNotFound;
                return response;
            }

            response.ResponseResult = GetAllChatsResult.Success;
            response.Value = _repository.ChatRepository.GetAllUsersChats(dto.Id);

            return response;
        }

        public async Task<Response<ChatEntity, GetChatResult>> GetChatById(FindChatDto dto)
        {
            var response = new Response<ChatEntity, GetChatResult>();

            var chat = await _repository.ChatRepository.GetChatByIdAsync(dto.Id);

            if (chat == null)
            {
                response.ResponseResult = GetChatResult.ChatNotFound;
            }
            else
            {
                response.ResponseResult = GetChatResult.Success;
                response.Value = chat;
            }
            
            return response;
        }

        public async Task<Response<ChatEntity, UpdateChatResult>> UpdateChat(UpdateChatDto dto)
        {
            var response = new Response<ChatEntity, UpdateChatResult>();

            if (dto.IsModified)
            {
                var chat = await _repository.ChatRepository.GetChatByIdAsync(dto.Id);

                if (chat == null)
                {
                    response.ResponseResult = UpdateChatResult.ChatNotFound;
                    return response;
                }

                // check title
                if (string.IsNullOrEmpty(dto.NewTitle)
                    || dto.NewTitle.Equals(chat.Title))
                {
                    response.ResponseResult = UpdateChatResult.ValidationError;
                    response.Messages = new List<string>
                    {
                        $"New title \"{dto.NewTitle}\" is empty or equal to old title \"{chat.Title}\""
                    };
                    return response;
                }

                chat.Title = dto.NewTitle;

                // Check Participants
                if (dto.RemovedParticipants.IsEntitiesExist<UserEntity>(_repository)
                    && dto.NewParticipants.IsEntitiesExist<UserEntity>(_repository))
                {
                    var currentParticipants = chat.Participants.Select(x => x.Id).ToList();

                    // remove if possible
                    if (currentParticipants.TrySubParticipants(dto.RemovedParticipants, out var result))
                    {
                        chat.Participants = result.Select(x => _repository.UserRepository.GetUserById(x)).ToList();
                    }
                    else
                    {
                        response.ResponseResult = UpdateChatResult.DeletingParticipantsError;
                        response.Messages = new List<string>
                        {
                            $"Can't sub participants from current"
                        };

                    }

                    var sum = chat.Participants.Select(x => x.Id).ToList().AddParticipants(dto.NewParticipants);

                    chat.Participants = sum.Select(x => _repository.UserRepository.GetUserById(x)).ToList();

                    var updateResult = await _repository.ChatRepository.UpdateChat(chat);

                    if (updateResult != null)
                    {
                        response.Value = updateResult;
                        response.ResponseResult = UpdateChatResult.Success;
                        return response;
                    }
                }
                else 
                {
                    response.ResponseResult = UpdateChatResult.ParticipantsNotFound;
                    response.Messages = new List<string> 
                    {
                        $"Deleted or new participants not found"
                    };
                    return response;
                }
            }
            else 
            {
                response.ResponseResult = UpdateChatResult.NothingToUpdate;
            }

            return response;
        }
    }
}
