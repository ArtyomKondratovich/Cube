using Cube.Domain.Entities;
using Cube.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cube.DataAccess.Repositories.Friendship
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly CubeDbContext _dbContext;
        
        public FriendshipRepository(CubeDbContext context) 
        {
            _dbContext = context;
        }

        public async Task<FriendshipEntity?> CreateAsync(FriendshipEntity entity, CancellationToken token = default)
        {
            var createdFriendship = await _dbContext.Friendships.AddAsync(entity, token);

            if (createdFriendship != null)
            {
                await _dbContext.SaveChangesAsync(token);
                return createdFriendship.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(FriendshipEntity entity, CancellationToken token = default)
        {
            var deletedFriendship = _dbContext.Friendships.Remove(entity);

            if (deletedFriendship != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return deletedFriendship != null;
        }

        public async Task<IEnumerable<FriendshipEntity>> GetByFilterAsync(Expression<Func<FriendshipEntity, bool>> filter, CancellationToken token = default)
        {
            return await _dbContext.Friendships
                .Where(filter)
                .ToListAsync(token);
        }

        public async Task<FriendshipEntity?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Friendships
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<FriendshipEntity?> GetByPredicateAsync(Expression<Func<FriendshipEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.Friendships
                .FirstOrDefaultAsync(predicate, token);
        }

        // update for friendship is unnecessary
        public Task<FriendshipEntity?> UpdateAsync(FriendshipEntity entity, CancellationToken token = default)
        {
            throw new NotSupportedException();
        }
    }
}
