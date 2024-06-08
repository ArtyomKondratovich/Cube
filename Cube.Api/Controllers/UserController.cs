using Cube.Services.Services;
using Cube.Services.Services.User;
using Cube.Services.Services.User.Dto;
using Cube.Core.Entities;
using Cube.Core.Models.Friendship;
using Cube.Core.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoginResult = Cube.Services.Services.LoginResult;

namespace Cube.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [Route("login")]
        [HttpPost]
        public async Task<Response<UserAuthModel, LoginResult>> Login([FromBody] LoginDto dto)
        {
            return await _service.Login(dto);
        }

        [Route("register")]
        [HttpPost]
        public async Task<Response<bool, RegisterResult>> Register([FromForm] RegisterDto dto)
        {
            return await _service.Register(dto);
        }

        [Route("createFriendship")]
        [HttpPost]
        public async Task<Response<FriendshipModel, CreateFriendshipResult>> CreateFriendship([FromBody] FriendshipDto dto)
        {
            return await _service.CreateFriendshipAsync(dto);
        }

        [Authorize]
        [HttpPost]
        [Route("getUserFriends")]
        public async Task<Response<List<UserModel>, GetUserFriends>> GetFriends([FromBody] FindUserDto dto)
        {
            return await _service.GetUserFriendsAsync(dto);
        }

        [Authorize]
        [HttpPost]
        [Route("getAllUsers")]
        public async Task<Response<List<UserModel>, GetAllUsers>> GetAll() 
        {
            return await _service.GetAll();
        }

        [HttpPost]
        [Route("validateToken")]
        public Response<string, TokenValidationResult> ValidateToken([FromBody] TokenDto dto)
        {
            return _service.ValidateToken(dto);
        }

        [Authorize]
        [HttpPost]
        [Route("getUser")]
        public async Task<Response<UserModel, GetUserResult>> GetUser([FromBody] FindUserDto dto)
        {
            return await _service.GetUserById(dto);
        }
    }
}
