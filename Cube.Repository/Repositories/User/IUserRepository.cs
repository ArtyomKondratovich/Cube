using Cube.Core.Entities;
using Cube.Repository.Repositories;

namespace Cube.Repository.Repositories.User
{
    public interface IUserRepository : IRepositoryBase<UserEntity>
    {
        Task<List<UserEntity>> GetAll(CancellationToken token = default);
    }
}
