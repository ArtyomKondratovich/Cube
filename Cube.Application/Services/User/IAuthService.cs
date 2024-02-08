using Cube.Application.Services.User.Dto;
using Cube.Core.Models.User;

namespace Cube.Application.Services.User
{
    public interface IAuthService
    {
        public Task<Response<string, SignInResult>> SignIn(SignInDto dto);

        public Task<Response<AccountEntity, SignUpResult>> SignUp(SignUpDto dto);
    }
}
