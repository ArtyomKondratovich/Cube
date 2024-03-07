﻿using Cube.Core.Models;
using Cube.Core.Models.Chat;

namespace Cube.EntityFramework.Repository.Chat
{
    public interface IChatRepository
    {
        Task<ChatEntity?> GetChatByIdAsync(int id);
        ChatEntity? GetChatById(int id);
        List<ChatModel> GetAllUsersChats(int id);
        Task<ChatModel?> CreateChat(ChatEntity entity);
        Task<bool> DeleteChat(ChatEntity entity);
        Task<ChatModel?> UpdateChat(ChatEntity entity);
        ICollection<ChatEntity> GetEntitiesByIds(ICollection<int> ids);
    }
}