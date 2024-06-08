using Cube.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cube.Repository.Repositories.Chat
{
    public class ChatRepository : IChatRepository
    {
        private readonly CubeDbContext _dbContext;

        public ChatRepository(CubeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChatEntity?> CreateAsync(ChatEntity entity, CancellationToken token = default)
        {
            var createdChat = await _dbContext.Chats.AddAsync(entity, token);

            if (createdChat != null)
            {
                await _dbContext.SaveChangesAsync(token);

                return createdChat.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(ChatEntity entity, CancellationToken token = default)
        {
            var deletedEntity = _dbContext.Chats.Remove(entity);

            if (deletedEntity != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return deletedEntity != null;
        }

        public async Task<ChatEntity?> GetByPredicateAsync(Expression<Func<ChatEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.Chats.FirstOrDefaultAsync(predicate, token);
        }

        public async Task<ChatEntity?> UpdateAsync(ChatEntity entity, CancellationToken token = default)
        {
            var updatedChat = _dbContext.Chats.Update(entity);

            if (updatedChat != null)
            {
                await _dbContext.SaveChangesAsync(token);
                return updatedChat.Entity;
            }

            return null;
        }

        public async Task<IEnumerable<ChatEntity>> GetByFilterAsync(Expression<Func<ChatEntity, bool>> filter, CancellationToken token = default)
        {
            return await _dbContext.Chats.Where(filter).ToListAsync(token);
        }

        public async Task<ChatEntity?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Chats.FirstOrDefaultAsync(x => x.Id == id, token);
        }
    }
}