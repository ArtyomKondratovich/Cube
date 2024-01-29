using Cube.Application.Services;
using Cube.Application.Services.User;
using Cube.Application.Services.User.Dto;
using Cube.Core.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Cube.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [Route("login")]
        [HttpPost]
        public async Task<Response<string, LoginResult>> Login([FromBody] LoginDto dto)
        {
            return await _service.Login(dto);
        }

        [Route("register")]
        [HttpPost]
        public async Task<Response<AccountModel, RegisterResult>> Register([FromBody] RegisterDto dto)
        {
            return await _service.Register(dto);
        }
    }
}
