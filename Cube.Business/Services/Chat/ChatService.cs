using Cube.Business.Services.Chat.Dto;
using Cube.Business.Utilities;
using Cube.Domain.Entities;
using Cube.Domain.Enums;
using Cube.Domain.Models;
using Cube.Domain.Models.Chat;
using Cube.Domain.Models.User;
using Cube.DataAccess.Repositories;
using System.Linq.Expressions;
using AutoMapper;
using Cube.Business.Utilities.Extensions;

namespace Cube.Business.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly Mapper _mapper;

        public ChatService(IRepositoryWrapper repository,
            Mapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
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

                var chat = _mapper.Map<ChatEntity>(dto);

                chat.Users = dto.PatricipantsIds.Select(async x => await _repository.UserRepository.GetByIdAsync(x))
                   .Select(t => t.Result)
                   .Where(i => i != null)
                   .ToList();

                var result = await _repository.ChatRepository.CreateAsync(chat);

                if (result != null)
                {
                    responce.ResponseResult = CreateChatResult.Success;
                    var chatModel = _mapper.Map<ChatModel>(result);

                    foreach (var user in result.Users)
                    {
                        var userModel = _mapper.Map<UserModel>(user);
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

            var chat = await _repository.ChatRepository.GetByIdAsync(dto.Id);
            var user = await _repository.UserRepository.GetByIdAsync(dto.UserId);

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

            if (!chat.Users.Any() && await _repository.ChatRepository.DeleteAsync(chat))
            {
                response.ResponseResult = DeleteChatResult.Success;
                response.Value = true;
                return response;
            }

            if (await _repository.ChatRepository.UpdateAsync(chat) != null)
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

            if (await _repository.UserRepository.GetByIdAsync(dto.Id) == null)
            {
                response.ResponseResult = GetAllChatsResult.UserNotFound;
                return response;
            }

            Expression<Func<ChatEntity, bool>> filter = chat => chat.Users.Select(x => x.Id).Contains(dto.Id);

            var chats = await _repository.ChatRepository.GetByFilterAsync(filter);

            foreach (var chat in chats) 
            {

                var chatModel = _mapper.Map<ChatModel>(chat);
                chatModel.Users = new();

                foreach (var user in chat.Users)
                {
                    var userModel = _mapper.Map<UserModel>(user);
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

            var chat = await _repository.ChatRepository.GetByIdAsync(dto.Id);

            if (chat == null)
            {
                response.ResponseResult = GetChatResult.ChatNotFound;
            }
            else
            {
                var chatModel = _mapper.Map<ChatModel>(chat);
                chatModel.Users = new();
                
                foreach (var user in chat.Users)
                {
                    var userModel = _mapper.Map<UserModel>(user);
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
                var chat = await _repository.ChatRepository.GetByIdAsync(dto.Id);

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

                    var newChatUsers = (await Task
                        .WhenAll(dto.NewParticipants!
                        .Select(x => _repository.UserRepository.GetByIdAsync(x)))
                        )
                        .Where(u => u != null)
                        .Cast<UserEntity>()
                        .ToList();


                    foreach (var item in newChatUsers)
                    {
                        chat.Users.Add(item);
                    }

                    var updatedChat = await _repository.ChatRepository.UpdateAsync(chat);

                    if (updatedChat != null)
                    {
                        var chatModel = _mapper.Map<ChatModel>(updatedChat);

                        foreach (var user in updatedChat.Users)
                        {
                            var userModel = _mapper.Map<UserModel>(user);
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
            if (await _repository.UserRepository.GetByIdAsync(id) == null)
            {
                return null;
            }

            Expression<Func<ImageEntity, bool>> predicate = image => image.OwnerId == id && image.Type == ImageType.Profile;

            var image = await _repository.ImageRepository.GetByPredicateAsync(predicate);

            if (image == null)
            {
                return null;
            }

            var imageBytes = await File.ReadAllBytesAsync(image.Path);

            return imageBytes;
        }
    }
}
