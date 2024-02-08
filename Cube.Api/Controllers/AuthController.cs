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

        [Route("login")]
        [HttpPost]
        public async Task<Response<string, SignInResult>> Login([FromBody] SignInDto dto)
        {
            return await _service.Login(dto);
        }

        [Route("register")]
        [HttpPost]
        public async Task<Response<AccountEntity, SignUpResult>> Register([FromBody] SignUpDto dto)
        {
            return await _service.Register(dto);
        }
    }
}
