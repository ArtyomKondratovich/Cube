﻿using Cube.Core.Models;

namespace Cube.EntityFramework.Repository.Message
{
    public interface IMessageRepository
    {
        Task<MessageModel?> GetMessageById(int id);
        Task<MessageModel?> SendMessage(MessageModel model);
        Task<MessageModel?> UpdateMessage(MessageModel model);
        Task<MessageModel?> DeleteMessage(MessageModel model);
    }
}