using Cube.Services.Services.Notification.dto;
using Cube.Services.Services.User.Dto;
using Cube.Core.Models.Notification;

namespace Cube.Services.Services.Notification
{
    public interface INotificationService
    {
        Task<Response<UserNotifications, GetUserNotificationResult>> GetUserNotifications(FindUserDto dto);
        Task<Response<bool, CreateNotificationResult>> CreateNotification(NewNotificationDto dto);
        Task<Response<bool, DeleteNotificationResult>> DeleteReadedNotification(ReadedNotificationDto dto);
    }
}
