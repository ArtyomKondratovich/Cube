using Cube.Domain.Models;
using Cube.DataAccess.Repositories;
using System.Linq.Expressions;

namespace Cube.DataAccess.Repositories.Message
{
    public interface IMessageRepository : IRepositoryBase<MessageEntity> 
    {
        Task<IEnumerable<MessageEntity>> GetChatMessagesAsync(int chatId, int take, int skip, CancellationToken token = default);
    }
}
