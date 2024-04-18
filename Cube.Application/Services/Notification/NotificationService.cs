using Cube.Application.Services.Notification.dto;
using Cube.Application.Services.User.Dto;
using Cube.Core.Entities;
using Cube.Core.Enums;
using Cube.Core.Models.Notification;
using Cube.EntityFramework.Repository;
using System.Linq.Expressions;

namespace Cube.Application.Services.Notification
{
    public class NotificationService : INotificationService
    {
        public readonly IRepositoryWrapper _repository;

        public NotificationService(IRepositoryWrapper repository) 
        {
            _repository = repository;
        }

        public async Task<Response<bool, CreateNotificationResult>> CreateNotification(NewNotificationDto dto)
        {
            var response = new Response<bool, CreateNotificationResult>();

            if (await IsNotificationSenderExists(dto.NotificationSenderId, dto.Type))
            {
                if (await dto.UserIds.IsEntitiesExist<UserEntity>(_repository))
                {
                    var notificationEntities = dto.UserIds
                        .Select(x => new NotificationEntity
                        {
                            UserId = x,
                            NotificationSenderId = dto.NotificationSenderId,
                            IsReaded = false,
                            Type = dto.Type,
                            Accepted = dto.Accepted
                        })
                        .ToList();

                    foreach (var notification in notificationEntities)
                    {
                        await _repository.NotificationRepository.CreateAsync(notification);
                    }

                    response.ResponseResult = CreateNotificationResult.Success;
                    response.Value = true;
                }
                else 
                {
                    response.ResponseResult = CreateNotificationResult.UsersNotFound;
                }
            }
            else
            {
                response.ResponseResult = CreateNotificationResult.NotificationSenderNotFound;
            }

            return response;    
        }

        public async Task<Response<bool, DeleteNotificationResult>> DeleteReadedNotification(ReadedNotificationDto dto)
        {
            var response = new Response<bool, DeleteNotificationResult>();

            if (dto.ReadedNotificationTds.Count > 0)
            {
                var notifications = dto.ReadedNotificationTds
                    .Select(async notificationId => await _repository.NotificationRepository.GetByIdAsync(notificationId))
                    .Select(x => x.Result)
                    .ToList();

                foreach (var notification in notifications)
                {
                    if (notification != null) 
                    {
                        await _repository.NotificationRepository.DeleteAsync(notification);
                    }
                }
            }

            response.ResponseResult = DeleteNotificationResult.Success;
            response.Value = true;
            return response;
        }

        public async Task<Response<UserNotifications, GetUserNotificationResult>> GetUserNotifications(FindUserDto dto)
        {
            var response = new Response<UserNotifications, GetUserNotificationResult>();

            var user = await _repository.UserRepository.GetByIdAsync(dto.Id);

            if (user == null)
            {
                response.ResponseResult = GetUserNotificationResult.UserNotFound;
            }
            else 
            {
                Expression<Func<NotificationEntity, bool>> filter = notification => notification.UserId == user.Id;
                response.Value = new UserNotifications
                {
                    Notifications = await _repository.NotificationRepository.GetByFilterAsync(filter)
                };
                response.ResponseResult = GetUserNotificationResult.Success;
            }

            return response;
        }

        private async Task<bool> IsNotificationSenderExists(int id, NotificationType type)
            => type switch
            {
                NotificationType.FriendRequest => await _repository.UserRepository.GetByIdAsync(id) != null,
                NotificationType.FriendResponse => await _repository.UserRepository.GetByIdAsync(id) != null,
                NotificationType.NewsNotification => throw new NotSupportedException(),
                NotificationType.ChatNotification => await _repository.ChatRepository.GetByIdAsync(id) != null,
                _ => throw new NotSupportedException()
            };
    }
}
