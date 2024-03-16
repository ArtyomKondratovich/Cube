using Cube.Application.Services.User.Auth;
using Cube.Application.Services.User.Dto;
using Cube.Application.Utilities;
using Cube.Core.Entities;
using Cube.Core.Models.Friendship;
using Cube.Core.Models.User;
using Cube.EntityFramework.Repository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cube.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IOptions<AuthOptions> _authOptions;

        public UserService(IRepositoryWrapper repository, IOptions<AuthOptions> options)
        {
            _repository = repository;
            _authOptions = options;
        }

        public async Task<Response<FriendshipModel, CreateFriendshipResult>> CreateFriendshipAsync(FriendshipDto dto)
        {
            var response = new Response<FriendshipModel, CreateFriendshipResult>();

            var user = await _repository.UserRepository.GetUserByIdAsync(dto.UserId);
            var friend = await _repository.UserRepository.GetUserByIdAsync(dto.FriendId);

            if (user == null) 
            {
                response.ResponseResult = CreateFriendshipResult.UserNotFound; 
                return response;
            }

            if (friend == null) 
            {
                response.ResponseResult = CreateFriendshipResult.FriendNotFound;
                return response;
            }

            var friendshipEntity = MapperConfig.InitializeAutomapper().Map<FriendshipEntity>(dto);
            friendshipEntity.Friend = friend;
            friendshipEntity.User = user;

            var result = await _repository.FriendshipRepository.CreateFriendshipAsync(friendshipEntity);

            if (result == null) 
            {
                response.ResponseResult = CreateFriendshipResult.Unsuccess;
                return response;
            }

            response.ResponseResult = CreateFriendshipResult.Success;
            response.Value = MapperConfig.InitializeAutomapper().Map<FriendshipModel>(result);
            return response;
        }

        public Task<Response<UserEntity, DeleteUserResult>> DeleteUser(DeleteUserDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<UserEntity, GetUserResult>> GetUserById(FindUserDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<UserAuthModel, LoginResult>> Login(LoginDto dto)
        {
            var response = new Response<UserAuthModel, LoginResult>()
            {
                Value = new UserAuthModel()
            };

            var user = await _repository.UserRepository.UserAssociatedWithTheEmail(dto.Email);

            if (user != null)
            {
                var role = await _repository.RoleRepository.GetRoleByIdAsync(user.RoleId);

                if (role != null)
                {
                    var hash = dto.Password.GetHash();

                    if (hash.Equals(user.Password))
                    {
                        var token = GenerateJWT(user, role.Name);
                        response.Value.Token = token;
                        response.Value.User = user;
                        response.ResponseResult = LoginResult.Success;
                    }
                    else
                    {
                        response.ResponseResult = LoginResult.WrongLoginOrPassword;
                    }

                    return response;
                }
            }

            response.ResponseResult = LoginResult.WrongLoginOrPassword;

            return response;
        }

        public async Task<Response<bool, RegisterResult>> Register(RegisterDto dto)
        {
            var response = new Response<bool, RegisterResult>();
            var existUser = await _repository.UserRepository.UserAssociatedWithTheEmail(dto.Email);

            if (existUser != null)
            {
                response.ResponseResult = RegisterResult.EmailAlreadyExists;
                return response;
            }

            if (!dto.Email.IsValidEmail())
            {
                response.ResponseResult = RegisterResult.ValidationError;
                response.Messages = new List<string>()
                {
                    $"'{dto.Email}' is not an email"
                };
                return response;
            }

            var password = dto.Password.PasswordCheck();

            if (password == 0)
            {
                response.ResponseResult = RegisterResult.ValidationError;
                response.Messages = new List<string>()
                {
                    "password is null"
                };
                return response;
            }

            var user = MapperConfig.InitializeAutomapper().Map<UserEntity>(dto);
            var role = await _repository.RoleRepository.GetRoleByNameAsync("User");

            if (role == null)
            {
                response.ResponseResult = RegisterResult.ValidationError;
                return response;
            }

            user.RoleId = role.Id;

            var result = await _repository.UserRepository.CreteUserAsync(user);

            if (result == null)
            {
                response.ResponseResult = RegisterResult.DataBaseError;
                response.Messages = new List<string>()
                {
                    "Writing to the database isn't successful"
                };
                return response;
            }

            response.Value = true;
            response.ResponseResult = RegisterResult.Success;

            return response;
        }

        public Task<Response<UserEntity, UpdateUserResult>> UpdateUser(UpdateUserDto dto)
        {
            throw new NotImplementedException();
        }

        private string GenerateJWT(UserEntity user, string role)
        {
            var authParams = _authOptions.Value;

            var securityKey = authParams.SummetricKey;
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, role),
            };

            var token = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.UtcNow.AddSeconds(authParams.TokenLifetime),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
