using Cube.Application.Services.Image.Dto;
using Cube.Application.Utilities;
using Cube.Core.Entities;
using Cube.Core.Enums;
using Cube.Core.Models;
using Cube.EntityFramework.Repository;

namespace Cube.Application.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly string _imagesDirectoryPath;

        public ImageService(IRepositoryWrapper repository, string imageDirectoryPath) 
        {
            _repository = repository;
            _imagesDirectoryPath = imageDirectoryPath;
        }

        public async Task<Response<ImageModel, CreateImageResult>> CreateImage(NewImageDto dto)
        {
            var response = new Response<ImageModel, CreateImageResult>();

            if (!await IsOwnerExist(dto.Type, dto.OwnerId))
            {
                response.ResponseResult = CreateImageResult.OwnerNotFound;
                return response;
            }

            if (dto.ImgBytes.Length == 0)
            {
                response.ResponseResult = CreateImageResult.CurruptedFile;
                return response;
            }

            var path = Path.Combine(_imagesDirectoryPath, $"\\{dto.Type}\\");

            if (!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }

            var fileName = $"{dto.Type}_{dto.OwnerId}.png";

            if (File.Exists(path + fileName))
            {
                response.ResponseResult = CreateImageResult.ImageAlreadyExist;
                return response;
            }

            using (FileStream fileStream = File.Create(path + fileName)) 
            {
                dto.File.CopyTo(fileStream);
                fileStream.Flush();
            }
            
            var entity = MapperConfig.InitializeAutomapper().Map<ImageEntity>(dto);
            entity.Path = path + fileName;

            if (await _repository.ImageRepository.CreateImageAsync(entity) == null
                || !File.Exists(entity.Path))
            {
                response.ResponseResult = CreateImageResult.ServerError;
                return response;
            }

            response.ResponseResult = CreateImageResult.Success;
            response.Value = MapperConfig.InitializeAutomapper().Map<ImageModel>(entity);
            return response;
        }

        public Task<Response<bool, CreateImageResult>> DeleteImageAsync(DeleteImageDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<ImageModel, GetImageResult>> GetImageByTypeAndOwnerAsync(FindImageDto dto)
        {
            var response = new Response<ImageModel, GetImageResult>();

            if (!await IsOwnerExist(dto.Type, dto.OwnerId))
            {
                response.ResponseResult = GetImageResult.OwnerNotFound;
                return response;
            }

            var entity = await _repository.ImageRepository.GetImageByTypeAndOwnerAsync(dto.Type, dto.OwnerId);

            if (entity == null) 
            {
                response.ResponseResult = GetImageResult.Success;
                var defaultImage = await _repository.ImageRepository.GetImageByTypeAndOwnerAsync(dto.Type, 0);
                response.Value = MapperConfig.InitializeAutomapper().Map<ImageModel>(defaultImage);
                return response;
            }

            response.ResponseResult = GetImageResult.Success;
            response.Value = MapperConfig.InitializeAutomapper().Map<ImageModel>(entity);
            return response;
        }

        public Task<Response<ImageModel, CreateImageResult>> UpdateImageAsync(UpdateImageDto dto)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> IsOwnerExist(ImageType type, int ownerId) => type switch
        {
            ImageType.Profile => await _repository.UserRepository
                .GetUserByIdAsync(ownerId) != null,
            ImageType.Chat => await _repository.ChatRepository
                .GetChatByIdAsync(ownerId) != null,
            ImageType.Post => throw new NotImplementedException(),
            _ => throw new NotSupportedException()
        };

        
         
    }
}
