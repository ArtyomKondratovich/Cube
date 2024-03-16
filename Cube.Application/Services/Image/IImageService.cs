using Cube.Application.Services.Image.Dto;
using Cube.Core.Entities;

namespace Cube.Application.Services.Image
{
    public interface IImageService
    {
        Task<Response<ImageEntity, OperationResult>> CreateImage(NewImageDto dto);
        Task<Response<bool, OperationResult>> DeleteImageAsync(DeleteImageDto dto);
        Task<Response<bool, OperationResult>> UpdateImageAsync(UpdateImageDto dto);
        Task<Response<ImageEntity, OperationResult>> GetImageByNameAsync(FindImageDto dto);

    }
}
