using Cube.Core.Entities;
using Cube.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Cube.Repository.Repository.Notification
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly CubeDbContext _context;

        public NotificationRepository(CubeDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationEntity?> CreateNotification(NotificationEntity entity)
        {
            var result = await _context.Notifications.AddAsync(entity);

            if (result != null) 
            {
                await _context.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteNotification(NotificationEntity entity)
        {
            var result = _context.Notifications.Remove(entity);

            if (result != null) 
            {
                await _context.SaveChangesAsync();
            }

            return result != null;
        }

        public async Task<NotificationEntity?> GetNotificationById(int id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<NotificationEntity>> GetUserNotifications(int userId)
        {
            return _context.Notifications
                .Where(x => x.UserId == userId && !x.IsReaded)
                .ToListAsync();
        }
    }
}
