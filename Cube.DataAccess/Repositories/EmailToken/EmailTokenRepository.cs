using Cube.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cube.DataAccess.Repositories.EmailToken
{
    public class EmailTokenRepository : IEmailTokenRepository
    {
        private readonly CubeDbContext _dbContext;

        public EmailTokenRepository(CubeDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task ClearExpiredTokensAsync(CancellationToken token = default)
        {
            var expiredTokens = _dbContext.EmailTokens.Where(x => x.ExpiredAt < DateTime.UtcNow);
            _dbContext.EmailTokens.RemoveRange(expiredTokens);
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task<EmailTokenEntity?> CreateAsync(EmailTokenEntity entity, CancellationToken token = default)
        {
            var createdToken = await _dbContext.EmailTokens.AddAsync(entity, token);

            if (createdToken != null)
            {
                await _dbContext.SaveChangesAsync(token);
                return createdToken.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(EmailTokenEntity entity, CancellationToken token = default)
        {
            var deletedToken = _dbContext.EmailTokens.Remove(entity);

            if (deletedToken != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return deletedToken != null;
        }

        public async Task<IEnumerable<EmailTokenEntity>> GetByFilterAsync(Expression<Func<EmailTokenEntity, bool>> filter, CancellationToken token = default)
        {
            return await _dbContext.EmailTokens
                .Where(filter)
                .ToListAsync(token);
        }

        public async Task<EmailTokenEntity?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.EmailTokens
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<EmailTokenEntity?> GetByPredicateAsync(Expression<Func<EmailTokenEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.EmailTokens
                .FirstOrDefaultAsync(predicate, token);
        }

        //update for token is unnecessary
        public Task<EmailTokenEntity?> UpdateAsync(EmailTokenEntity entity, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
