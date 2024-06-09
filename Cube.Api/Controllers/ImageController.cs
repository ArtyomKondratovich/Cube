using Microsoft.AspNetCore.Mvc;
using Cube.Business.Services.Image;
using Cube.Business.Services;
using Cube.Domain.Models.Image;
using Cube.Business.Utilities;
using Cube.Business.Services.Image.Dto;

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
