using Cube.Server.Models;
using Cube.Server.Models.Dto;
using Cube.Server.Models.ResultObjects;

namespace Cube.Server.Repository.Interfaces
{
    public interface IUserRepository
    {
        Result GetUserById(FindUserDto dto);
    }
}
