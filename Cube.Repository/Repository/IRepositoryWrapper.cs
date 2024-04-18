using Cube.EntityFramework.Repository.Chat;
using Cube.EntityFramework.Repository.Message;
using Cube.EntityFramework.Repository.User;
using Cube.Repository.Repository.Config;
using Cube.Repository.Repository.Friendship;
using Cube.Repository.Repository.Image;
using Cube.Repository.Repository.Notification;
using Cube.Repository.Repository.Role;

namespace Cube.EntityFramework.Repository
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
