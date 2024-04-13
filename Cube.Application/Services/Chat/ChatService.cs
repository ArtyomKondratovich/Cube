using Cube.Application.Services.Chat.Dto;
using Cube.Application.Utilities;
using Cube.Core.Entities;
using Cube.Core.Enums;
using Cube.Core.Models;
using Cube.Core.Models.Chat;
using Cube.Core.Models.User;
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

                chat.Users = dto.PatricipantsIds.Select(async x => await _repository.UserRepository.GetUserByIdAsync(x))
                   .Select(t => t.Result)
                   .Where(i => i != null)
                   .ToList();

                var result = await _repository.ChatRepository.CreateChat(chat);

                if (result != null)
                {
                    responce.ResponseResult = CreateChatResult.Success;
                    var chatModel = MapperConfig.InitializeAutomapper().Map<ChatModel>(result);

                    foreach (var user in result.Users)
                    {
                        var userModel = MapperConfig.InitializeAutomapper().Map<UserModel>(user);
                        userModel.AvatarBytes = await GetUserAvatar(userModel.Id);
                        chatModel.Users.Add(userModel);
                    }

                    responce.Value = chatModel;
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

            chat.Users = chat.Users
                .Where(x => x.Id != dto.UserId)
                .ToList();

            if (!chat.Users.Any() && await _repository.ChatRepository.DeleteChat(chat.Id))
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

        public async Task<Response<List<ChatModel>, GetAllChatsResult>> GetAllUserChats(FindUserChatsDto dto)
        {
            var response = new Response<List<ChatModel>, GetAllChatsResult> 
            { 
                Value = new()
            };

            if (await _repository.UserRepository.GetUserByIdAsync(dto.Id) == null)
            {
                response.ResponseResult = GetAllChatsResult.UserNotFound;
                return response;
            }

            var chats = await _repository.ChatRepository.GetAllUsersChatsAsync(dto.Id);

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
                    response.Value.Add(result.Value);
                    response.ResponseResult = GetAllChatsResult.Success;
                    return response;
                }
            }

            foreach (var chat in chats) 
            {

                var chatModel = MapperConfig.InitializeAutomapper().Map<ChatModel>(chat);
                chatModel.Users = new();

                foreach (var user in chat.Users)
                {
                    var userModel = MapperConfig.InitializeAutomapper().Map<UserModel>(user);
                    userModel.AvatarBytes = await GetUserAvatar(userModel.Id);
                    chatModel.Users.Add(userModel);
                }

                response.Value.Add(chatModel);
            }

            response.ResponseResult = GetAllChatsResult.Success;

            return response;
        }

        public async Task<Response<ChatModel, GetChatResult>> GetChatById(FindChatDto dto)
        {
            var response = new Response<ChatModel, GetChatResult>();

            var chat = await _repository.ChatRepository.GetChatByIdAsync(dto.Id);

            if (chat == null)
            {
                response.ResponseResult = GetChatResult.ChatNotFound;
            }
            else
            {
                var chatModel = MapperConfig.InitializeAutomapper().Map<ChatModel>(chat);
                chatModel.Users = new();
                
                foreach (var user in chat.Users)
                {
                    var userModel = MapperConfig.InitializeAutomapper().Map<UserModel>(user);
                    userModel.AvatarBytes = await GetUserAvatar(userModel.Id);
                    chatModel.Users.Add(userModel);
                }

                response.Value = chatModel;
                response.ResponseResult = GetChatResult.Success;
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
                    // remove if possible
                    if (chat.Users
                        .Select(x => x.Id)
                        .ToList()
                        .TrySubParticipants(dto.RemovedParticipants, out var result))
                    {
                        chat.Users = chat.Users
                            .Where(x => result!.Contains(x.Id))
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

                    var newChatUsers = dto.NewParticipants!
                        .Select(async x => await _repository.UserRepository.GetUserByIdAsync(x))
                        .Select(t => t.Result)
                        .ToList();

                    foreach (var item in newChatUsers)
                    {
                        chat.Users.Add(item);
                    }

                    var updateResult = await _repository.ChatRepository.UpdateChat(chat);

                    if (updateResult != null)
                    {
                        var chatModel = MapperConfig.InitializeAutomapper().Map<ChatModel>(updateResult);

                        foreach (var user in updateResult.Users)
                        {
                            var userModel = MapperConfig.InitializeAutomapper().Map<UserModel>(user);
                            userModel.AvatarBytes = await GetUserAvatar(userModel.Id);
                            chatModel.Users.Add(userModel);
                        }

                        response.Value = chatModel;
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

        private async Task<byte[]?> GetUserAvatar(int id)
        {
            if (await _repository.UserRepository.GetUserByIdAsync(id) == null)
            {
                return null;
            }

            var image = await _repository.ImageRepository.GetImageByTypeAndOwnerAsync(ImageType.Profile, id);

            if (image == null)
            {
                return null;
            }

            var imageBytes = await File.ReadAllBytesAsync(image.Path);

            return imageBytes;
        }
    }
}
