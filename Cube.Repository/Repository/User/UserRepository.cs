using Cube.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cube.EntityFramework.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly CubeDbContext _dbContext;

        public UserRepository(CubeDbContext context) 
        {
            _dbContext = context;
        }

        public async Task<List<UserEntity>> GetAll(CancellationToken token = default)
        {
            return await _dbContext.Users.ToListAsync(token);
        }

        public async Task<UserEntity?> CreateAsync(UserEntity entity, CancellationToken token = default)
        {
            var createdUser = await _dbContext.Users.AddAsync(entity, token);

            if (createdUser != null)
            {
                await _dbContext.SaveChangesAsync(token);
                return createdUser.Entity;
            }

            return null;
        }

        public async Task<UserEntity?> UpdateAsync(UserEntity entity, CancellationToken token = default)
        {
            var updatedUser = _dbContext.Users.Update(entity);

            if (updatedUser != null) 
            {
                await _dbContext.SaveChangesAsync(token);
                return updatedUser.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(UserEntity entity, CancellationToken token = default)
        {
            var deletedUser = _dbContext.Users.Remove(entity);

            if (deletedUser != null) 
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return deletedUser != null;
        }

        public async Task<UserEntity?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<UserEntity?> GetByPredicateAsync(Expression<Func<UserEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(predicate, token);
        }

        public async Task<IEnumerable<UserEntity>> GetByFilterAsync(Expression<Func<UserEntity, bool>> filter, CancellationToken token = default)
        {
            return await _dbContext.Users
                .Where(filter)
                .ToListAsync(token);
        }
    }
}
