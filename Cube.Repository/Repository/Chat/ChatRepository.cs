using Cube.Core.Models;
using Cube.Core.Models.Chat;
using Microsoft.EntityFrameworkCore;

namespace Cube.EntityFramework.Repository.Chat
{
    public class ChatRepository : IChatRepository
    {
        private readonly CubeDbContext _dbContext;

        public ChatRepository(CubeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChatModel?> CreateChat(ChatEntity entity)
        {
            var chat = await _dbContext.Chats.AddAsync(entity);

            if (chat != null)
            {
                await _dbContext.SaveChangesAsync();

                return new ChatModel 
                { 
                    Id = chat.Entity.Id,
                    Title = chat.Entity.Title,
                    Type = chat.Entity.Type
                };
            }
            
            return null;

        }

        public async Task<bool> DeleteChat(int id)
        {
            var chat = await _dbContext.Chats
                .FirstAsync(c => c.Id == id);

            if (_dbContext.Chats.Remove(chat) != null) 
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<ChatModel>> GetAllUsersChatsAsync(int id)
        {
            return await _dbContext.Chats
                .Where(c => c.Users.Where(x => x.Id == id).Any())
                .Select(x => new ChatModel 
                {
                    Id = x.Id,
                    Title = x.Title,
                    Type = x.Type
                })
                .ToListAsync();
        }

        public ChatEntity? GetChatById(int id)
        {
            return _dbContext.Chats
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<ChatEntity?> GetChatByIdAsync(int id)
        {
            return await _dbContext.Chats
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public ICollection<ChatEntity> GetEntitiesByIds(ICollection<int> ids)
        {
            var entities = new List<ChatEntity>();

            foreach (var id in ids)
            {
                var entity = _dbContext.Chats.Find(id);

                if (entity != null)
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public async Task<ChatModel?> UpdateChat(ChatEntity entity)
        {
            var result = _dbContext.Chats.Update(entity);

            if (result != null)
            {
                await _dbContext.SaveChangesAsync();
                return new ChatModel 
                {
                    Id = result.Entity.Id,
                    Title = result.Entity.Title,
                    Type = result.Entity.Type
                };
            }

            return null;
        }
    }
}