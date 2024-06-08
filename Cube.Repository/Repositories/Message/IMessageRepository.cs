using Cube.Core.Models;
using Cube.Repository.Repositories;
using System.Linq.Expressions;

namespace Cube.Repository.Repositories.Message
{
    public interface IMessageRepository : IRepositoryBase<MessageEntity> 
    {
        Task<IEnumerable<MessageEntity>> GetChatMessagesAsync(int chatId, int take, int skip, CancellationToken token = default);
    }
}
