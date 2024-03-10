using Cube.Core.Entities;

namespace Cube.Repository.Repository.Role
{
    public interface IRoleRepository
    {
        Task<RoleEntity?> GetRoleByIdAsync(int id);
        Task<RoleEntity?> GetRoleByNameAsync(string roleName);
        Task<RoleEntity?> CreateRoleAsync(RoleEntity entity);
    }
}
