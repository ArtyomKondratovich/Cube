using Cube.Business.Services.Image.Dto;
using Cube.Business.Utilities;
using Cube.Domain.Entities;
using Cube.Domain.Enums;
using Cube.Domain.Models.Image;
using Cube.DataAccess.Repositories;
using System.Linq.Expressions;
using AutoMapper;

namespace Cube.Business.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly string _imagesDirectoryPath;
        private readonly Mapper _mapper;

        public ImageService(IRepositoryWrapper repository, 
            string imagesDirectoryPath,
            Mapper mapper) 
        {
            _repository = repository;
            _imagesDirectoryPath = imagesDirectoryPath;
            _mapper = mapper;
        }

        public async Task<Response<ImageModel, CreateImageResult>> CreateImage(NewImageDto dto)
        {
            var response = new Response<ImageModel, CreateImageResult>();

            if (dto == null)
            {
                Console.WriteLine();
                return response;
            }

            if (!await IsOwnerExist(dto.Type, dto.OwnerId))
            {
                response.ResponseResult = CreateImageResult.OwnerNotFound;
                return response;
            }

            var path = _imagesDirectoryPath + $"\\{dto.Type}\\";

            if (!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }

            var fileName = $"{dto.Type}_{dto.OwnerId}.png";

            var fullPath = path + fileName;

            Console.WriteLine(fullPath);

            if (File.Exists(fullPath))
            {
                response.ResponseResult = CreateImageResult.ImageAlreadyExist;
                return response;
            }

            using (FileStream fileStream = File.Create(fullPath)) 
            {
                dto.File.CopyTo(fileStream);
                fileStream.Flush();
            }
            
            var entity = _mapper.Map<ImageEntity>(dto);
            entity.Path = fullPath;

            if (await _repository.ImageRepository.CreateAsync(entity) == null
                || !File.Exists(entity.Path))
            {
                response.ResponseResult = CreateImageResult.ServerError;
                return response;
            }

            response.ResponseResult = CreateImageResult.Success;
            response.Value = _mapper.Map<ImageModel>(entity);
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

            Expression<Func<ImageEntity, bool>> predicate = image => image.OwnerId == dto.OwnerId && image.Type == dto.Type;

            var entity = await _repository.ImageRepository.GetByPredicateAsync(predicate);
            
            if (entity != null) 
            {
                var model = _mapper.Map<ImageModel>(entity);
                model.ImgBytes = File.ReadAllBytes(entity.Path);
                response.Value = model;
            }

            response.ResponseResult = GetImageResult.Success;
            return response;
        }

        public Task<Response<ImageModel, CreateImageResult>> UpdateImageAsync(UpdateImageDto dto)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> IsOwnerExist(ImageType type, int ownerId) => type switch
        {
            ImageType.Profile => await _repository.UserRepository
                .GetByIdAsync(ownerId) != null,
            ImageType.Chat => await _repository.ChatRepository
                .GetByIdAsync(ownerId) != null,
            ImageType.Post => throw new NotImplementedException(),
            _ => throw new NotSupportedException()
        };

    }
}
