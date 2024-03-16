using Cube.EntityFramework.Repository.Chat;
using Cube.EntityFramework.Repository.Message;
using Cube.EntityFramework.Repository.User;
using Cube.Repository.Repository.Friendship;
using Cube.Repository.Repository.Image;
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
        private IFriendshipRepository _friendshipRepository;
        private IImageRepository _imageRepository;

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

        public IFriendshipRepository FriendshipRepository 
        {
            get 
            {
                _friendshipRepository ??= new FriendshipRepository(_dbContext);

                return _friendshipRepository;
            }
        }

        public IImageRepository ImageRepository 
        {
            get 
            {
                _imageRepository ??= new ImageRepository(_dbContext);

                return _imageRepository;
            }
        }

        public RepositoryWrapper(CubeDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
