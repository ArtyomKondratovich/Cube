using Cube.Core.Entities;
using Cube.Core.Enums;
using Cube.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Cube.Repository.Repository.Image
{
    public class ImageRepository : IImageRepository
    {
        private readonly CubeDbContext _context;

        public ImageRepository(CubeDbContext context) 
        {
            _context = context;
        }

        public async Task<ImageEntity> CreateImageAsync(ImageEntity entity)
        {
            var result = await _context.Images.AddAsync(entity);

            if (result != null)
            {
                await _context.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }

        public async Task<bool> DeleteImageAsync(int id)
        {
            var image = _context.Images
                .FirstOrDefault(x => x.Id == id);

            if (image != null) 
            {
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
            }

            return image != null;
        }

        public async Task<ImageEntity> GetImageByTypeAndOwnerAsync(ImageType type, int ownerId)
        {
            return await _context.Images
                .FirstOrDefaultAsync(x => x.Type == type && x.OwnerId == ownerId);
        }

        public async Task<ImageEntity> UpdateImageAsync(ImageEntity entity)
        {
            var result = _context.Images.Update(entity);

            if (result != null) 
            {
                await _context.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }
    }
}
