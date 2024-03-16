using Cube.Core.Entities;

namespace Cube.Repository.Repository.Image
{
    public interface IImageRepository
    {
        Task<ImageEntity> GetImageByNameAsync(string name);
        Task<ImageEntity> CreateImageAsync(ImageEntity entity);
        Task<bool> UpdateImageAsync(ImageEntity entity);
        Task<bool> DeleteImageAsync(string name);
    }
}
