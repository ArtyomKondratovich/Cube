using Cube.Core.Entities;
using Cube.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Cube.Repository.Repository.Role
{
    public class RoleRepository : IRoleRepository
    {
        private readonly CubeDbContext _dbContext;

        public RoleRepository(CubeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RoleEntity?> CreateRoleAsync(RoleEntity entity)
        {
            var result = await _dbContext.Roles.AddAsync(entity);

            if (result != null) 
            {
                await _dbContext.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }

        public async Task<RoleEntity?> GetRoleByIdAsync(int id)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<RoleEntity?> GetRoleByNameAsync(string roleName)
        {
            return await _dbContext.Roles
                .FirstAsync(x => x.Name == roleName);
        }
    }
}
