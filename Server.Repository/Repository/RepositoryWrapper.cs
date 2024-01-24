using Cube.Application.Repository.Message;
using Cube.EntityFramework.Repository.Chat;
using Cube.EntityFramework.Repository.User;

namespace Cube.EntityFramework.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private CubeDbContext _dbContext;
        private IUserRepository _userRepository;
        private IChatRepository _chatRepository;
        private IMessageRepository _messageRepository;

        public IUserRepository UserRepository 
        {
            get
            {
                _userRepository ??= new UserRerository(_dbContext);

                return _userRepository;
            }
        }

        public IMessageRepository MessageRepository
        {
            get
            {
                _messageRepository ??= new MessageRepository(_dbContext);

                return _messageRepository;
            }
        }

        public IChatRepository ChatRepository
        {
            get
            {
                _chatRepository ??= new ChatRepository(_dbContext);

                return _chatRepository;
            }
        }

        public RepositoryWrapper(CubeDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
