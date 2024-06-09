using Cube.Domain.Entities;
using Cube.Domain.Enums;
using Cube.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq.Expressions;

namespace Cube.DataAccess.Repositories.Image
{
    public class ImageRepository : IImageRepository
    {
        private readonly CubeDbContext _dbContext;

        public ImageRepository(CubeDbContext context) 
        {
            _dbContext = context;
        }

        public async Task<ImageEntity?> CreateAsync(ImageEntity entity, CancellationToken token = default)
        {
            var createdImage = await _dbContext.Images.AddAsync(entity, token);

            if (createdImage != null)
            {
                await _dbContext.SaveChangesAsync(token);
                return createdImage.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(ImageEntity entity, CancellationToken token = default)
        {
            var deletedImage = _dbContext.Images.Remove(entity);

            if (deletedImage != null) 
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return deletedImage != null;
        }

        public async Task<IEnumerable<ImageEntity>> GetByFilterAsync(Expression<Func<ImageEntity, bool>> filter, CancellationToken token = default)
        {
            return await _dbContext.Images
                .Where(filter)
                .ToListAsync(token);
        }

        public async Task<ImageEntity?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Images
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<ImageEntity?> GetByPredicateAsync(Expression<Func<ImageEntity, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.Images
                .FirstOrDefaultAsync(predicate, token);
        }

        public async Task<ImageEntity?> UpdateAsync(ImageEntity entity, CancellationToken token = default)
        {
            var updatedImage = _dbContext.Images.Update(entity);

            if (updatedImage != null)
            {
                await _dbContext.SaveChangesAsync(token);
                return updatedImage.Entity;
            }

            return null;
        }
    }
}
