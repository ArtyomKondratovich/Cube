using Cube.Application.Services.User.Dto;
using Cube.Core.Models.User;

namespace Cube.Application.Services.User
{
    public interface IAuthService
    {
        public Task<Response<string, SignInResult>> Login(SignInDto dto);

        public Task<Response<AccountEntity, SignUpResult>> Register(SignUpDto dto);
    }
}
