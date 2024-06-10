using Cube.Domain.Entities;

namespace Cube.DataAccess.Repositories.EmailToken
{
    public interface IEmailTokenRepository : IRepositoryBase<EmailTokenEntity>
    {
        public Task ClearExpiredTokensAsync(CancellationToken token = default);
    }
}
