using Cube.EntityFramework;
using Cube.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Cube.EntityFramework.Repository.Message
{
    public class MessageRepository : IMessageRepository
    {
        private readonly CubeDbContext _dbContext;

        public MessageRepository(CubeDbContext contex)
        {
            _dbContext = contex;
        }

        public async Task<MessageModel?> DeleteMessage(MessageModel model)
        {
            var message = _dbContext.Messages.Remove(model);

            if (message != null)
            {
                await _dbContext.SaveChangesAsync();

                return message.Entity;
            }

            return null;
        }

        public async Task<MessageModel?> GetMessageById(int id)
        {
            return await _dbContext.Messages.FindAsync(id);
        }

        public async Task<MessageModel?> SendMessage(MessageModel model)
        {
            var message = await _dbContext.Messages.AddAsync(model);

            if (message != null)
            {
                await _dbContext.SaveChangesAsync();

                return message.Entity;
            }

            return null;
        }

        public async Task<MessageModel?> UpdateMessage(MessageModel model)
        {
            var message = _dbContext.Messages.Update(model);

            if (message != null)
            {
                await _dbContext.SaveChangesAsync();

                return message.Entity;
            }

            return null;
        }
    }
}