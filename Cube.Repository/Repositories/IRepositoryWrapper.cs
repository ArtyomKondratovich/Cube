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
    public interface IRepositoryWrapper
    {
        public IUserRepository UserRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public IChatRepository ChatRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IFriendshipRepository FriendshipRepository { get; }
        public IImageRepository ImageRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IConfigRepositrory ConfigRepositrory { get; }
    }
}
