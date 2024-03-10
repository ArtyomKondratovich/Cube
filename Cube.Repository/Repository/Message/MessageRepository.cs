using Cube.Core.Models;

namespace Cube.EntityFramework.Repository.Message
{
    public class MessageRepository : IMessageRepository
    {
        private readonly CubeDbContext _dbContext;

        public MessageRepository(CubeDbContext contex)
        {
            _dbContext = contex;
        }

        public async Task<MessageEntity?> DeleteMessage(MessageEntity model)
        {
            var message = _dbContext.Messages.Remove(model);

            if (message != null)
            {
                await _dbContext.SaveChangesAsync();

                return message.Entity;
            }

            return null;
        }

        public async Task<MessageEntity?> GetMessageById(int id)
        {
            return await _dbContext.Messages.FindAsync(id);
        }

        public async Task<MessageEntity?> SendMessage(MessageEntity model)
        {
            var message = await _dbContext.Messages.AddAsync(model);

            if (message != null)
            {
                await _dbContext.SaveChangesAsync();

                return message.Entity;
            }

            return null;
        }

        public async Task<MessageEntity?> UpdateMessage(MessageEntity model)
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