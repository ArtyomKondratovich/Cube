using Cube.Services.Services.Config.dto;
using Cube.Core.Models.Config;

namespace Cube.Services.Services.Config
{
    public interface IConfigService
    {
        Task<Response<ConfigModel, CreateConfigResult>> Create(NewConfigDto dto);
        Task<Response<ConfigModel, UpdateConfigResult>> Update(UpdateConfigDto dto);
        Task<Response<bool, DeleteConfigResult>> Delete(DeleteConfigDto dto);
        Task<Response<ConfigModel, GetConfigResult>> Get(FindConfigDto dto);
    }
}
