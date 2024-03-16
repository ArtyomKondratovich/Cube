using Cube.Application.Services.User.Dto;
using Cube.Core.Entities;
using Cube.Core.Models.Friendship;
using Cube.Core.Models.User;

namespace Cube.Application.Services.User
{
    public interface IUserService
    {
        public Task<Response<UserAuthModel, LoginResult>> Login(LoginDto dto);
        public Task<Response<bool, RegisterResult>> Register(RegisterDto dto);
        public Task<Response<UserEntity, GetUserResult>> GetUserById(FindUserDto dto);
        public Task<Response<UserEntity, DeleteUserResult>> DeleteUser(DeleteUserDto dto);
        public Task<Response<UserEntity, UpdateUserResult>> UpdateUser(UpdateUserDto dto);
        public Task<Response<FriendshipModel, CreateFriendshipResult>> CreateFriendshipAsync(FriendshipDto dto);
    }
}
