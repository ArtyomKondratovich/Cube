﻿using Cube.Application.Services.User.Dto;
using Cube.Core.Models.User;

namespace Cube.Application.Services.User
{
    public interface IUserService
    {
        public Task<Response<UserAuthModel, LoginResult>> Login(LoginDto dto);
        public Task<Response<bool, RegisterResult>> Register(RegisterDto dto);
        public Task<Response<UserEntity, GetUserResult>> GetUserById(FindUserDto dto);
        public Task<Response<UserEntity, CreateUserResult>> CreateUser(NewUserDto dto);
        public Task<Response<UserEntity, DeleteUserResult>> DeleteUser(DeleteUserDto dto);
        public Task<Response<UserEntity, UpdateUserResult>> UpdateUser(UpdateUserDto dto);
    }
}