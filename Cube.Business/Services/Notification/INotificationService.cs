using Cube.Business.Services.Notification.dto;
using Cube.Business.Services.User.Dto;
using Cube.Domain.Models.Notification;
using Cube.Business.Utilities;

namespace Cube.Business.Services.Notification
{
    public interface INotificationService
    {
        Task<Response<UserNotifications, GetUserNotificationResult>> GetUserNotifications(FindUserDto dto);
        Task<Response<bool, CreateNotificationResult>> CreateNotification(NewNotificationDto dto);
        Task<Response<bool, DeleteNotificationResult>> DeleteReadedNotification(ReadedNotificationDto dto);
    }
}
