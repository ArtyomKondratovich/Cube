using Cube.Application.Services.User.Dto;
using Cube.Core.Models.User;

namespace Cube.Application.Services.User
{
    public interface IUserService
    {
        public Task<Response<string, LoginResult>> Login(LoginDto dto);

        public Task<Response<AccountModel, RegisterResult>> Register(RegisterDto dto);
    }
}
