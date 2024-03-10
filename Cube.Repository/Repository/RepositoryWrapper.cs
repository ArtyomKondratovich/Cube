using Cube.EntityFramework.Repository.Chat;
using Cube.EntityFramework.Repository.Message;
using Cube.EntityFramework.Repository.User;
using Cube.Repository.Repository.Role;

namespace Cube.EntityFramework.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private CubeDbContext _dbContext;
        private IUserRepository _userRepository;
        private IChatRepository _chatRepository;
        private IMessageRepository _messageRepository;
        private IRoleRepository _roleRepository;

        public IUserRepository UserRepository 
        {
            get
            {
                _userRepository ??= new UserRepository(_dbContext);

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

        public IRoleRepository RoleRepository
        {
            get
            {
                _roleRepository ??= new RoleRepository(_dbContext);

                return _roleRepository;
            }
        }

        public RepositoryWrapper(CubeDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
