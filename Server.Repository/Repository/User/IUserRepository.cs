using Cube.Core.Models;

namespace Cube.EntityFramework.Repository.User
{
    public interface IUserRepository
    {
        Task<UserModel?> GetUserById(int id);
    }
}
