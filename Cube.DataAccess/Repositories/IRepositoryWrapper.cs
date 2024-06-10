using Cube.DataAccess.Repositories.Chat;
using Cube.DataAccess.Repositories.Message;
using Cube.DataAccess.Repositories.User;
using Cube.DataAccess.Repositories.Config;
using Cube.DataAccess.Repositories.Friendship;
using Cube.DataAccess.Repositories.Image;
using Cube.DataAccess.Repositories.Notification;
using Cube.DataAccess.Repositories.Role;
using Cube.DataAccess.Repositories.EmailToken;

namespace Cube.DataAccess.Repositories
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
        public IEmailTokenRepository EmailTokenRepository { get; }
    }
}
