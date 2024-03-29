﻿using Cube.Application.Services;
using Cube.Application.Services.User;
using Cube.Application.Services.User.Dto;
using Cube.Core.Entities;
using Cube.Core.Models.Friendship;
using Cube.Core.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoginResult = Cube.Application.Services.LoginResult;

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
        public async Task<Response<bool, RegisterResult>> Register([FromBody] RegisterDto dto)
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
        public async Task<Response<List<UserEntity>, GetUserFriends>> GetFriends([FromBody] FindUserDto dto)
        {
            return await _service.GetUserFriendsAsync(dto);
        }

        [Authorize]
        [HttpPost]
        [Route("getAllUsers")]
        public async Task<Response<List<UserEntity>, GetAllUsers>> GetAll() 
        {
            return await _service.GetAll();
        }
    }
}
