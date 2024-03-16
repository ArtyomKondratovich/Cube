using Cube.Application.Services.Image.Dto;
using Cube.Core.Entities;
using Cube.EntityFramework.Repository;

namespace Cube.Application.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IRepositoryWrapper _repository;

        public ImageService(IRepositoryWrapper repository) 
        {
            _repository = repository;
        }

        public async Task<Response<ImageEntity, OperationResult>> CreateImage(NewImageDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool, OperationResult>> DeleteImageAsync(DeleteImageDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ImageEntity, OperationResult>> GetImageByNameAsync(FindImageDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool, OperationResult>> UpdateImageAsync(UpdateImageDto dto)
        {
            throw new NotImplementedException();
        }

        private static int[] GetIdsFromImageName(string name)
        {
            var ids = new int[2];

            var parts = name.Split('_');

            for (var i = 0; i < 2; i++)
            {
                ids[i] = int.Parse(parts[i + 1]);
            }

            return ids;
        }
    }
}
