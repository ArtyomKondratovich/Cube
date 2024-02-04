using Cube.Application.Services;
using Cube.Application.Services.User;
using Cube.Application.Services.User.Dto;
using Cube.Core.Models.User;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Cube.Application.Services.SignInResult;

namespace Cube.Web.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [Route("signin")]
        [HttpPost]
        public async Task<Response<string, SignInResult>> SignIn([FromBody] SignInDto dto)
        {
            return await _service.SignIn(dto);
        }

        [Route("signup")]
        [HttpPost]
        public async Task<Response<AccountModel, SignUpResult>> SignUp([FromBody] SignUpDto dto)
        {
            return await _service.SignUp(dto);
        }
    }
}
