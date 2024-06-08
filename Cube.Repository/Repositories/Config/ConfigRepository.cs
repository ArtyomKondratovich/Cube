using Cube.Core.Entities;
using Cube.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cube.Repository.Repositories.Config
{
    public class ConfigRepository : IConfigRepositrory
    {
        private readonly CubeDbContext _dbContext;

        public ConfigRepository(CubeDbContext context) 
        {
            _dbContext = context;
        }

        public async Task<ConfigEntity?> CreateAsync(ConfigEntity entity, CancellationToken token = default)
        {
            var createdChat = _dbContext.Configs.Add(entity);

            if (createdChat != null)
            {
                await _dbContext.SaveChangesAsync(token);
                return createdChat.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(ConfigEntity entity, CancellationToken token = default)
        {
            var deletedConfig = _dbContext.Configs.Remove(entity);

            if (deletedConfig != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return deletedConfig != null;
        }

        public async Task<IEnumerable<ConfigEntity>> GetByFilterAsync(Expression<Func<ConfigEntity, bool>> filter, CancellationToken token = default)
        {
            return await _dbContext.Configs.Where(filter).ToListAsync(token);
        }

        public async Task<ConfigEntity?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Configs.FirstOrDefaultAsync(x => x.UserId == id, token);
        }

        public async Task<ConfigEntity?> GetByPredicateAsync(Expression<Func<ConfigEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.Configs.FirstOrDefaultAsync(predicate, token);
        }

        public async Task<ConfigEntity?> UpdateAsync(ConfigEntity entity, CancellationToken token = default)
        {
            var updatedConfig = _dbContext.Configs.Update(entity);

            if (updatedConfig != null) 
            {
                await _dbContext.SaveChangesAsync(token);
                return updatedConfig.Entity;
            }

            return null;
        }
    }
}
