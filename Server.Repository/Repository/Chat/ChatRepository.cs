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

        public async Task<bool> CreateChat(ChatModel model)
        {
            var chat = await _dbContext.Chats.AddAsync(model);

            if (chat != null)
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteChat(ChatModel model)
        {
            var chat = _dbContext.Chats.Remove(model);

            if (chat != null) 
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public List<ChatModel> GetAllUsersChats(int id)
        {
            var usersChats = _dbContext.Chats.Where(c => c.Participants.Select(user => user.Id).Contains(id)).ToList();

            return usersChats;
        }
    }
}