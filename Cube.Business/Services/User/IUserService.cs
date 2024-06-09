using Cube.Business.Services.User.Dto;
using Cube.Domain.Models.Friendship;
using Cube.Domain.Models.User;
using Cube.Business.Utilities;

namespace Cube.Business.Services.User
{
    public interface IUserService
    {
        public Task<Response<UserAuthModel, LoginResult>> Login(LoginDto dto);
        public Task<Response<bool, RegisterResult>> Register(RegisterDto dto);
        public Task<Response<UserModel, GetUserResult>> GetUserById(FindUserDto dto);
        public Task<Response<bool, DeleteUserResult>> DeleteUser(DeleteUserDto dto);
        public Task<Response<UserModel, UpdateUserResult>> UpdateUser(UpdateUserDto dto);
        public Task<Response<FriendshipModel, CreateFriendshipResult>> CreateFriendshipAsync(FriendshipDto dto);
        public Task<Response<List<UserModel>, GetUserFriends>> GetUserFriendsAsync(FindUserDto dto);
        public Task<Response<List<UserModel>, GetAllUsers>> GetAll();
        public Response<string, TokenValidationResult> ValidateToken(TokenDto dto);
    }
}
