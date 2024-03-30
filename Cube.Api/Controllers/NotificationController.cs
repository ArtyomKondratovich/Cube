using Cube.Application.Services;
using Cube.Application.Services.Notification;
using Cube.Application.Services.Notification.dto;
using Cube.Application.Services.User.Dto;
using Cube.Core.Models.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service) 
        {
            _service = service;
        }

        [Route("get")]
        [HttpPost]
        public async Task<Response<UserNotifications, GetUserNotificationResult>> GetNotifications([FromBody] FindUserDto dto)
        {
            return await _service.GetUserNotifications(dto);
        }

        [Route("delete")]
        [HttpPost]
        public async Task<Response<bool, DeleteNotificationResult>> DeleteNotifications([FromBody] ReadedNotificationDto dto)
        {
            return await _service.DeleteReadedNotification(dto);
        }

        [Route("create")]
        [HttpPost]
        public async Task<Response<bool, CreateNotificationResult>> CreateNotifications([FromBody] NewNotificationDto dto)
        {
            return await _service.CreateNotification(dto);
        }
    }
}
