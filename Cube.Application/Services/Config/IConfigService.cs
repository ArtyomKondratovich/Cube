using Cube.Application.Services.Config.dto;
using Cube.Core.Models.Config;

namespace Cube.Application.Services.Config
{
    public interface IConfigService
    {
        Task<Response<ConfigModel, CreateConfigResult>> Create(NewConfigDto dto);
        Task<Response<ConfigModel, UpdateConfigResult>> Update(UpdateConfigDto dto);
        Task<Response<bool, DeleteConfigResult>> Delete(DeleteConfigDto dto);
        Task<Response<ConfigModel, GetConfigResult>> Get(FindConfigDto dto);
    }
}
