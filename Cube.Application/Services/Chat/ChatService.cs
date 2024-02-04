using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;
using Cube.Application.Utilities;
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
                if (!dto.PatricipantsIds.IsEntitiesExist<UserModel, IRepositoryWrapper>(_repository))
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

                var chat = MapperConfig.InitializeAutomapper(_repository).Map<ChatModel>(dto);

                if (!isProperlyCofigured) 
                {
                    responce.ResponseResult = CreateChatResult.WrongChatSettings;
                    return responce;
                }

               

                if (await _repository.ChatRepository.CreateChat(chat) != null)
                {
                    responce.ResponseResult = CreateChatResult.Success;
                    responce.Value = chat;
                    return responce;
                }
            }

            return responce;
        }

        public async Task<Response<ChatModel, DeleteChatResult>> DeleteChat(DeleteChatDto dto)
        {
            var response = new Response<ChatModel, DeleteChatResult>();

            var chat = await _repository.ChatRepository.GetChatById(dto.Id);
            var user = await _repository.UserRepository.GetUserById(dto.Id);

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
            
            return response;
        }

        public async Task<Response<List<ChatModel>, GetAllChatsResult>> GetAll(FindUserDto dto)
        {

            var response = new Response<List<ChatModel>, GetAllChatsResult>();

            if (await _repository.UserRepository.GetUserById(dto.Id) == null)
            {
                response.ResponseResult = GetAllChatsResult.UserNotFound;
                return response;
            }

            response.ResponseResult = GetAllChatsResult.Success;
            response.Value = _repository.ChatRepository.GetAllUsersChats(dto.Id);

            return response;
        }

        public async Task<Response<ChatModel, GetChatResult>> GetChatById(FindChatDto dto)
        {
            var response = new Response<ChatModel, GetChatResult>();

            var chat = await _repository.ChatRepository.GetChatById(dto.Id);

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

        public Task<Response<ChatModel, UpdateChatResult>> UpdateChat(UpdateChatDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
