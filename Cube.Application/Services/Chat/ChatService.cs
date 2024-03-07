using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;
using Cube.Application.Utilities;
using Cube.Core.Models;
using Cube.Core.Models.Chat;
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

        public async Task<Response<ChatModel, CreateChatResult>> CreateChat(NewChatDto dto)
        {
            var responce = new Response<ChatModel, CreateChatResult>();

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
                if (!await dto.PatricipantsIds.IsEntitiesExist<UserEntity>(_repository))
                {
                    responce.ResponseResult = CreateChatResult.ParticipantsNotFound; 
                    return responce;
                }

                var chat = MapperConfig.InitializeAutomapper().Map<ChatEntity>(dto);

                chat.Participants = dto.PatricipantsIds
                    .Select(id => _repository.UserRepository.GetUserById(id)!)
                    .ToList();

                var result = await _repository.ChatRepository.CreateChat(chat);

                if (result != null)
                {
                    responce.ResponseResult = CreateChatResult.Success;
                    responce.Value = result;
                    return responce;
                }
            }

            return responce;
        }

        public async Task<Response<bool, DeleteChatResult>> DeleteChat(DeleteChatDto dto)
        {
            var response = new Response<bool, DeleteChatResult>();

            var chat = await _repository.ChatRepository.GetChatByIdAsync(dto.Id);
            var user = await _repository.UserRepository.GetUserByIdAsync(dto.UserId);

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
                case ChatDeletionType.RemoveFromChatList:
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
            }

            chat.Participants = currentParticipants
                        .Select(x => _repository.UserRepository.GetUserById(x)!)
                        .ToList();

            if (!chat.Participants.Any() && await _repository.ChatRepository.DeleteChat(chat))
            {
                response.ResponseResult = DeleteChatResult.Success;
                response.Value = true;
                return response;
            }

            if (await _repository.ChatRepository.UpdateChat(chat) != null)
            {
                response.ResponseResult = DeleteChatResult.Success;
                response.Value = true;
            }

            return response;
        }

        public async Task<Response<List<ChatModel>, GetAllChatsResult>> GetAllUsersChats(FindUserDto dto)
        {
            var response = new Response<List<ChatModel>, GetAllChatsResult>();

            if (await _repository.UserRepository.GetUserByIdAsync(dto.Id) == null)
            {
                response.ResponseResult = GetAllChatsResult.UserNotFound;
                return response;
            }

            var chats = _repository.ChatRepository.GetAllUsersChats(dto.Id);

            if (!chats.Any())
            {
                // default adding SavedMessage chat
                var savedMessages = new NewChatDto 
                {
                    Title = "Saved Messages",
                    Type = ChatType.SavedMessages,
                    PatricipantsIds = new List<int> { dto.Id }
                };

                var result = await CreateChat(savedMessages);

                if (result.ResponseResult == CreateChatResult.Success)
                {
                    chats.Add(new ChatModel
                    {
                        Id = result.Value.Id,
                        Title = result.Value.Title,
                        Type = ChatType.SavedMessages,
                    });
                    response.ResponseResult = GetAllChatsResult.Success;
                    response.Value = chats;
                    return response;
                }
            }

            response.Value = chats;

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

        public async Task<Response<ChatModel, UpdateChatResult>> UpdateChat(UpdateChatDto dto)
        {
            var response = new Response<ChatModel, UpdateChatResult>();

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
                if (await dto.RemovedParticipants.IsEntitiesExist<UserEntity>(_repository)
                    && await dto.NewParticipants.IsEntitiesExist<UserEntity>(_repository))
                {
                    var currentParticipants = chat.Participants.Select(x => x.Id).ToList();

                    // remove if possible
                    if (currentParticipants.TrySubParticipants(dto.RemovedParticipants, out var result))
                    {
                        chat.Participants = result!
                            .Select(x => _repository.UserRepository.GetUserById(x)!)
                            .ToList();
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

                    chat.Participants = sum
                        .Select(x => _repository.UserRepository.GetUserById(x)!)
                        .ToList();

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
