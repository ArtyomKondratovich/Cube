using Cube.Core.Entities;

namespace Cube.Repository.Repository.Notification
{
    public interface INotificationRepository
    {
        Task<NotificationEntity?> CreateNotification(NotificationEntity entity);
        Task<bool> DeleteNotification(NotificationEntity entity);
        Task<NotificationEntity?> GetNotificationById(int id);
        Task<List<NotificationEntity>> GetUserNotifications(int userId);
    }
}
