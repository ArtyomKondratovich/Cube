using Cube.Core.Entities;
using Cube.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cube.Repository.Repositories.Role
{
    public class RoleRepository : IRoleRepository
    {
        private readonly CubeDbContext _dbContext;

        public RoleRepository(CubeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RoleEntity?> CreateAsync(RoleEntity entity, CancellationToken token = default)
        {
            var createdRole = await _dbContext.AddAsync(entity, token);

            if (createdRole != null) 
            {
                await _dbContext.SaveChangesAsync(token);
                return createdRole.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(RoleEntity entity, CancellationToken token = default)
        {
            var deletedRole = _dbContext.Roles.Remove(entity);

            if (deletedRole != null) 
            { 
                await _dbContext.SaveChangesAsync(token);
            }

            return deletedRole != null;
        }

        public async Task<IEnumerable<RoleEntity>> GetByFilterAsync(Expression<Func<RoleEntity, bool>> filter, CancellationToken token = default)
        {
            return await _dbContext.Roles
                .Where(filter)
                .ToListAsync(token);
        }

        public async Task<RoleEntity?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<RoleEntity?> GetByPredicateAsync(Expression<Func<RoleEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(predicate, token);
        }

        // unnessesary to update role
        public Task<RoleEntity?> UpdateAsync(RoleEntity entity, CancellationToken token = default)
        {
            throw new NotSupportedException();
        }
    }
}
