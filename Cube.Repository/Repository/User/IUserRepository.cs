using Cube.Core.Entities;

namespace Cube.EntityFramework.Repository.User
{
    public interface IUserRepository
    {
        Task<UserEntity> CreteUserAsync(UserEntity entity);
        Task<UserEntity> GetUserByIdAsync(int id);
        UserEntity GetUserById(int id);
        Task<UserEntity> UserAssociatedWithTheEmail(string email);
        Task<List<UserEntity>> GetAll();
    }
}
