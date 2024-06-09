using Cube.Business.Services.Image.Dto;
using Cube.Domain.Models.Image;
using Cube.Business.Utilities;

namespace Cube.Business.Services.Image
{
    public interface IImageService
    {
        Task<Response<ImageModel, CreateImageResult>> CreateImage(NewImageDto dto);
        Task<Response<bool, CreateImageResult>> DeleteImageAsync(DeleteImageDto dto);
        Task<Response<ImageModel, CreateImageResult>> UpdateImageAsync(UpdateImageDto dto);
        Task<Response<ImageModel, GetImageResult>> GetImageByTypeAndOwnerAsync(FindImageDto dto);
    }
}
