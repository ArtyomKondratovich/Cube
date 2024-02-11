using Cube.Core.Models.User;

namespace Cube.EntityFramework.Repository.User
{
    public interface IUserRepository
    {
        Task<UserEntity?> CreteUser(UserEntity user);
        Task<UserEntity?> GetUserByIdAsync(int id);
        UserEntity? GetUserById(int id);
        UserEntity? UserAssociatedWithTheAccount(int accountId);
    }
}
