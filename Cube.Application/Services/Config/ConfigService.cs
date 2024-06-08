using Cube.Services.Services.Config.dto;
using Cube.Services.Utilities;
using Cube.Core.Entities;
using Cube.Core.Models.Config;
using Cube.Repository.Repositories;
using Newtonsoft.Json;

namespace Cube.Services.Services.Config
{
    public class ConfigService : IConfigService
    {
        private readonly IRepositoryWrapper _repository;

        public async Task<Response<ConfigModel, CreateConfigResult>> Create(NewConfigDto dto)
        {
            var response = new Response<ConfigModel, CreateConfigResult>();

            var user = await _repository.UserRepository.GetByIdAsync(dto.UserId);

            if (user == null) 
            {
                response.ResponseResult = CreateConfigResult.UserNotFound;
                return response;
            }

            var entity = MapperConfig.InitializeAutomapper().Map<ConfigEntity>(dto);

            var config = await _repository.ConfigRepositrory.CreateAsync(entity);

            if (config == null) 
            {
                response.ResponseResult = CreateConfigResult.DatabaseError;
                return response;
            }

            response.Value = MapperConfig.InitializeAutomapper().Map<ConfigModel>(config);
            response.ResponseResult = CreateConfigResult.Success;
            return response;
        }

        public async Task<Response<bool, DeleteConfigResult>> Delete(DeleteConfigDto dto)
        {
            var response = new Response<bool, DeleteConfigResult>();

            var user = await _repository.UserRepository.GetByIdAsync(dto.UserId);
            var config = await _repository.ConfigRepositrory.GetByIdAsync(dto.UserId);

            if (user == null)
            {
                response.ResponseResult = DeleteConfigResult.UserNotFound;
                return response;
            }

            if (config == null)
            {
                response.ResponseResult = DeleteConfigResult.ConfigNotFound;
                return response;
            }

            var isConfigDeleted = await _repository.ConfigRepositrory.DeleteAsync(config);

            if (!isConfigDeleted)
            {
                response.ResponseResult = DeleteConfigResult.DatabaseError;
                return response;
            }

            response.Value = isConfigDeleted;
            response.ResponseResult = DeleteConfigResult.Success;
            return response;
        }

        public async Task<Response<ConfigModel, GetConfigResult>> Get(FindConfigDto dto)
        {
            var response = new Response<ConfigModel, GetConfigResult>();

            var user = await _repository.UserRepository.GetByIdAsync(dto.UserId);

            if (user == null)
            {
                response.ResponseResult = GetConfigResult.UserNotFound;
                return response;
            }

            var config = await _repository.ConfigRepositrory.GetByIdAsync(dto.UserId);

            if (config == null)
            {
                response.ResponseResult = GetConfigResult.ConfigNotFound;
                return response;
            }

            response.Value = MapperConfig.InitializeAutomapper().Map<ConfigModel>(config);
            response.ResponseResult = GetConfigResult.Success;
            return response;

        }

        public async Task<Response<ConfigModel, UpdateConfigResult>> Update(UpdateConfigDto dto)
        {
            var response = new Response<ConfigModel, UpdateConfigResult>();

            var user = await _repository.UserRepository.GetByIdAsync(dto.UserId);

            if (user == null)
            {
                response.ResponseResult = UpdateConfigResult.UserNotFound;
                return response;
            }

            if (!dto.NewConfig.TryParseJson())
            {
                response.ResponseResult = UpdateConfigResult.ValidationError;
                return response;
            }

            var config = await _repository.ConfigRepositrory.GetByIdAsync(dto.UserId);

            if (config == null)
            {
                response.ResponseResult = UpdateConfigResult.ConfigNotFound;
                return response;
            }

            config.Config = dto.NewConfig;

            var updatedConfig = _repository.ConfigRepositrory.UpdateAsync(config);

            if (updatedConfig == null)
            {
                response.ResponseResult = UpdateConfigResult.DatabaseError;
                return response;
            }

            response.Value = MapperConfig.InitializeAutomapper().Map<ConfigModel>(updatedConfig);
            response.ResponseResult = UpdateConfigResult.Success;
            return response;
        }
    }
}
