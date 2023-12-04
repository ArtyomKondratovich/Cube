using Cube.Server.Models;
using Cube.Server.Repository.Interfaces;

namespace Cube.Server.Repository.Implementations
{
    public class MessageRepository : RepositoryBase<MessageModel>, IMessageRepository
    {
        public MessageRepository(RepositoryContext contex) : 
            base(contex) 
        {
        }
    }
}
