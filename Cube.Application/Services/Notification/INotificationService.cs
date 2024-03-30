using Cube.Application.Services.Notification.dto;
using Cube.Application.Services.User.Dto;
using Cube.Core.Models.Notification;

namespace Cube.Application.Services.Notification
{
    public interface INotificationService
    {
        Task<Response<UserNotifications, GetUserNotificationResult>> GetUserNotifications(FindUserDto dto);
        Task<Response<bool, CreateNotificationResult>> CreateNotification(NewNotificationDto dto);
        Task<Response<bool, DeleteNotificationResult>> DeleteReadedNotification(ReadedNotificationDto dto);
    }
}
