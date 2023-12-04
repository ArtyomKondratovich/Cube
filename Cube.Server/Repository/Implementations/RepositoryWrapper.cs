using Cube.Server.Repository.Interfaces;

namespace Cube.Server.Repository.Implementations
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _context;
        private IUserRepository _userRepository;
        private IMessageRepository _messageRepository;

        public IUserRepository User { 
            get {
                _userRepository ??= new UserRepository(_context);

                return _userRepository;
            }
        }

        public IMessageRepository Message {
            get {
                _messageRepository ??= new MessageRepository(_context);

                return _messageRepository;
            }
        }

        public RepositoryWrapper(RepositoryContext context) 
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
