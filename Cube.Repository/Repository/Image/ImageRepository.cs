using Cube.Core.Entities;
using Cube.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

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

        public async Task<bool> DeleteImageAsync(string name)
        {
            var image = _context.Images
                .FirstOrDefault(x => x.Name == name);

            if (image != null) 
            {
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
            }

            return image != null;
        }

        public async Task<ImageEntity> GetImageByNameAsync(string name)
        {
            return await _context.Images
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<bool> UpdateImageAsync(ImageEntity entity)
        {
            var updateResult = _context.Images.Update(entity);

            if (updateResult != null) 
            {
                await _context.SaveChangesAsync();
            }

            return updateResult != null;
        }
    }
}
