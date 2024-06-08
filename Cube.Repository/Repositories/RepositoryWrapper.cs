using Cube.Repository.Repositories.Chat;
using Cube.Repository.Repositories.Message;
using Cube.Repository.Repositories.User;
using Cube.Repository.Repositories.Config;
using Cube.Repository.Repositories.Friendship;
using Cube.Repository.Repositories.Image;
using Cube.Repository.Repositories.Notification;
using Cube.Repository.Repositories.Role;

namespace Cube.Repository.Repositories
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
        private INotificationRepository _notificationRepository;
        private IConfigRepositrory _configRepository;

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

        public INotificationRepository NotificationRepository
        {
            get
            {
                _notificationRepository ??= new NotificationRepository(_dbContext);

                return _notificationRepository;
            }
        }

        public IConfigRepositrory ConfigRepositrory
        {
            get
            {
                _configRepository ??= new ConfigRepository(_dbContext);

                return _configRepository;
            }
        }

        public RepositoryWrapper(CubeDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
