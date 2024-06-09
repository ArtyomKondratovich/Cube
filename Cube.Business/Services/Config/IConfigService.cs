using Cube.Business.Services.Config.dto;
using Cube.Domain.Models.Config;
using Cube.Business.Utilities;

namespace Cube.Business.Services.Config
{
    public interface IConfigService
    {
        Task<Response<ConfigModel, CreateConfigResult>> Create(NewConfigDto dto);
        Task<Response<ConfigModel, UpdateConfigResult>> Update(UpdateConfigDto dto);
        Task<Response<bool, DeleteConfigResult>> Delete(DeleteConfigDto dto);
        Task<Response<ConfigModel, GetConfigResult>> Get(FindConfigDto dto);
    }
}
