using Cube.Domain.Entities;
using Cube.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cube.DataAccess.Repositories.Notification
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly CubeDbContext _dbContext;

        public NotificationRepository(CubeDbContext context)
        {
            _dbContext = context;
        }

        public async Task<NotificationEntity?> CreateAsync(NotificationEntity entity, CancellationToken token = default)
        {
            var createdNotification = await _dbContext.Notifications.AddAsync(entity, token);

            if (createdNotification != null)
            {
                await _dbContext.SaveChangesAsync(token);
                return createdNotification.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(NotificationEntity entity, CancellationToken token = default)
        {
            var deletedNotofocation = _dbContext.Notifications.Remove(entity);

            if (deletedNotofocation != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return deletedNotofocation != null;
        }

        public async Task<IEnumerable<NotificationEntity>> GetByFilterAsync(Expression<Func<NotificationEntity, bool>> filter, CancellationToken token = default)
        {
            return await _dbContext.Notifications
                .Where(filter)
                .ToListAsync(token);
        }

        public async Task<NotificationEntity?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Notifications
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<NotificationEntity?> GetByPredicateAsync(Expression<Func<NotificationEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.Notifications
                .FirstOrDefaultAsync(predicate, token);
        }

        // unnessesary to update notification
        public Task<NotificationEntity?> UpdateAsync(NotificationEntity entity, CancellationToken token = default)
        {
            throw new NotSupportedException();
        }
    }
}
