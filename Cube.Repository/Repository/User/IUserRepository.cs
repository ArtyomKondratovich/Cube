using Cube.Core.Models.User;

namespace Cube.EntityFramework.Repository.User
{
    public interface IUserRepository
    {
        Task<UserModel?> GetUserById(int id);
    }
}
