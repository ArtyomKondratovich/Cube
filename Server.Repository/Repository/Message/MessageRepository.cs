using Cube.EntityFramework;
using Cube.Core.Models;
using Cube.EntityFramework.Repository.Message.Dto;

namespace Cube.Application.Repository.Message
{
    public class MessageRepository : IMessageRepository
    {
        private readonly CubeDbContext _dbContex;

        public MessageRepository(CubeDbContext contex)
        {
            _dbContex = contex;
        }

        public async Task<bool> DeleteMessage(DeleteMessageDto dto)
        {
            var message = await _dbContex.Messages.FindAsync(dto.Id);

            if (message == null) 
            {
                return false;
            }

            _dbContex.Messages.Remove(message);

            return await _dbContex.SaveChangesAsync() > 0;
        }

        public async Task<MessageModel?> GetMessageById(int id)
        {
            return await _dbContex.Messages.FindAsync(id);
        }

        public async Task<bool> SendMessage(MessageModel model)
        {
            await _dbContex.Messages.AddAsync(model);

            return await _dbContex.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateMessage(MessageModel model)
        {
            _dbContex.Messages.Update(model);

            return await _dbContex.SaveChangesAsync() > 0;
        }
    }
}