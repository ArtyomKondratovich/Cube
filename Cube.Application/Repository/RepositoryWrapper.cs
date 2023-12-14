using Cube.EntityFramework;
using Cube.Application.Repository.User;
using Cube.Application.Repository.Message;
using Cube.Application.Repository.Chat;

namespace Cube.Application.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly CubeDbContext _context;
        private IUserRepository _userRepository;
        private IMessageRepository _messageRepository;
        private IChatRepository _chatRepository;

        public IUserRepository User
        {
            get
            {
                _userRepository ??= new UserRepository(_context);

                return _userRepository;
            }
        }

        public IMessageRepository Message
        {
            get
            {
                _messageRepository ??= new MessageRepository(_context);

                return _messageRepository;
            }
        }

        public IChatRepository Chat
        {
            get 
            {
                _chatRepository ??= new ChatRepository(_context);

                return _chatRepository;
            }
        }

        public RepositoryWrapper(CubeDbContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
