using Cube.Core.Models;

namespace Cube.EntityFramework.Repository.Chat
{
    public class ChatRepository : IChatRepository
    {
        private readonly CubeDbContext _dbContext;

        public ChatRepository(CubeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChatEntity?> CreateChat(ChatEntity model)
        {
            var chat = await _dbContext.Chats.AddAsync(model);

            if (chat != null)
            {
                await _dbContext.SaveChangesAsync();
             
                return chat.Entity;
            }

            return null;
        }

        public async Task<ChatEntity?> DeleteChat(ChatEntity model)
        {
            var chat = _dbContext.Chats.Remove(model);

            if (chat != null) 
            {
                await _dbContext.SaveChangesAsync();
                return chat.Entity;
            }

            return null;
        }

        public List<ChatEntity> GetAllUsersChats(int id)
        {
            var usersChats = _dbContext.Chats.Where(c => c.Participants.Select(user => user.Id).Contains(id)).ToList();

            return usersChats;
        }

        public ChatEntity? GetChatById(int id)
        {
            return _dbContext.Chats.Find(id);
        }

        public async Task<ChatEntity?> GetChatByIdAsync(int id)
        {
            return await _dbContext.Chats.FindAsync(id);
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

        public async Task<ChatEntity?> UpdateChat(ChatEntity entity)
        {
            var result = _dbContext.Chats.Update(entity);

            if (result != null)
            {
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }
    }
}