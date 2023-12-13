using Cube.EntityFramework;
using Cube.Application.Repository.User;
using Cube.Application.Repository.Message;

namespace Cube.Application.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly CubeDbContext _context;
        private IUserRepository _userRepository;
        private IMessageRepository _messageRepository;

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
