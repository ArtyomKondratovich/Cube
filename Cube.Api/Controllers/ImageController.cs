using Cube.Services.Services;
using Cube.Services.Services.Image;
using Cube.Services.Services.Image.Dto;
using Cube.Core.Models.Image;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Api.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _service;

        public ImageController(IImageService service) 
        {
            _service = service;
        }

        [Route("get")]
        [HttpPost]
        public async Task<Response<ImageModel, GetImageResult>> Get([FromBody] FindImageDto dto)
        {
            return await _service.GetImageByTypeAndOwnerAsync(dto);
        }

        [Route("create")]
        [HttpPost]
        public async Task<Response<ImageModel, CreateImageResult>> Create([FromForm] NewImageDto dto)
        {
            return await _service.CreateImage(dto);
        }
    }
}
