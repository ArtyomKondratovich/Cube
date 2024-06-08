using Cube.Services.Services.Image.Dto;
using Cube.Core.Models.Image;

namespace Cube.Services.Services.Image
{
    public interface IImageService
    {
        Task<Response<ImageModel, CreateImageResult>> CreateImage(NewImageDto dto);
        Task<Response<bool, CreateImageResult>> DeleteImageAsync(DeleteImageDto dto);
        Task<Response<ImageModel, CreateImageResult>> UpdateImageAsync(UpdateImageDto dto);
        Task<Response<ImageModel, GetImageResult>> GetImageByTypeAndOwnerAsync(FindImageDto dto);
    }
}
