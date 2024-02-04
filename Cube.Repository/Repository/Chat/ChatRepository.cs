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

        public async Task<ChatModel?> CreateChat(ChatModel model)
        {
            var chat = await _dbContext.Chats.AddAsync(model);

            if (chat != null)
            {
                await _dbContext.SaveChangesAsync();
             
                return chat.Entity;
            }

            return null;
        }

        public async Task<ChatModel?> DeleteChat(ChatModel model)
        {
            var chat = _dbContext.Chats.Remove(model);

            if (chat != null) 
            {
                await _dbContext.SaveChangesAsync();
                return chat.Entity;
            }

            return null;
        }

        public List<ChatModel> GetAllUsersChats(int id)
        {
            var usersChats = _dbContext.Chats.Where(c => c.Participants.Select(user => user.Id).Contains(id)).ToList();

            return usersChats;
        }

        public async Task<ChatModel?> GetChatById(int id)
        {
            return await _dbContext.Chats.FindAsync(id);
        }

        public async Task<ICollection<ChatModel>> GetEntitiesByIds(ICollection<int> ids)
        {
            var entities = new List<ChatModel>();

            foreach (var id in ids)
            {
                entities.Add(await _dbContext.Chats.FindAsync(id));
            }

            return entities;
        }
    }
}