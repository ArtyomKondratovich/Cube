using Cube.Domain.Entities;
using Cube.DataAccess.Repositories;

namespace Cube.DataAccess.Repositories.User
{
    public interface IUserRepository : IRepositoryBase<UserEntity>
    {
        Task<List<UserEntity>> GetAll(CancellationToken token = default);
    }
}
