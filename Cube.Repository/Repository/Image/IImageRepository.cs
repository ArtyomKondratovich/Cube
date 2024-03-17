using Cube.Core.Entities;
using Cube.Core.Enums;
using Cube.Core.Models;

namespace Cube.Repository.Repository.Image
{
    public interface IImageRepository
    {
        Task<ImageEntity> GetImageByTypeAndOwnerAsync(ImageType type, int ownerId);
        Task<ImageEntity> CreateImageAsync(ImageEntity entity);
        Task<ImageEntity> UpdateImageAsync(ImageEntity entity);
        Task<bool> DeleteImageAsync(int id);
    }
}
