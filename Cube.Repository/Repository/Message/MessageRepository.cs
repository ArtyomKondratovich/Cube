using Cube.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cube.EntityFramework.Repository.Message
{
    public class MessageRepository : IMessageRepository
    {
        private readonly CubeDbContext _dbContext;

        public MessageRepository(CubeDbContext contex)
        {
            _dbContext = contex;
        }

        public async Task<MessageEntity?> CreateAsync(MessageEntity entity, CancellationToken token = default)
        {
            var createdMessage = await _dbContext.Messages.AddAsync(entity, token);

            if (createdMessage != null)
            {
                await _dbContext.SaveChangesAsync(token);

                return createdMessage.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(MessageEntity entity, CancellationToken token = default)
        {
            var deletedMessage = _dbContext.Messages.Remove(entity);

            if (deletedMessage != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return deletedMessage != null;
        }

        public async Task<IEnumerable<MessageEntity>> GetByFilterAsync(Expression<Func<MessageEntity, bool>> filter, CancellationToken token = default)
        {
            return await _dbContext.Messages
                .Where(filter)
                .ToListAsync(token);
        }

        public async Task<MessageEntity?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Messages
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<MessageEntity?> GetByPredicateAsync(Expression<Func<MessageEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.Messages
                .FirstOrDefaultAsync(predicate, token);
        }

        public async Task<MessageEntity?> UpdateAsync(MessageEntity entity, CancellationToken token = default)
        {
            var updatedMessage = _dbContext.Messages.Update(entity);

            if (updatedMessage != null)
            {
                await _dbContext.SaveChangesAsync(token);

                return updatedMessage.Entity;
            }

            return null;
        }
    }
}