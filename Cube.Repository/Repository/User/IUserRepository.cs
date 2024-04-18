using Cube.Core.Entities;
using Cube.Repository.Repository;

namespace Cube.EntityFramework.Repository.User
{
    public interface IUserRepository : IRepositoryBase<UserEntity>
    {
        Task<List<UserEntity>> GetAll(CancellationToken token = default);
    }
}
