using Cube.Core.Models.User;

namespace Cube.EntityFramework.Repository.User
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetUserByIdAsync(int id);
        UserEntity? GetUserById(int id);
    }
}
