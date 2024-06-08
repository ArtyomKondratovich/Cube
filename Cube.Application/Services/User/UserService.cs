using Cube.Services.Services.User.Auth;
using Cube.Services.Services.User.Dto;
using Cube.Services.Utilities;
using Cube.Core.Entities;
using Cube.Core.Enums;
using Cube.Core.Models.Friendship;
using Cube.Core.Models.User;
using Cube.Repository.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Cube.Core.Models.Image;
using Cube.Core.Models;
using System.Linq.Expressions;

namespace Cube.Services.Services.User
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
            
            Expression<Func<FriendshipEntity, bool>> filter = friendship => friendship.FirstUserId == dto.UserId || friendship.SecondUserId == dto.UserId;

            var friendships = await _repository.FriendshipRepository.GetByFilterAsync(filter);

            foreach (var friendship in friendships) 
            {
                if (friendship.FirstUserId == dto.FriendId || friendship.SecondUserId == dto.FriendId) 
                {
                    response.ResponseResult = CreateFriendshipResult.FriendshipAlreadyExist;
                    return response;
                }
            }

            var firstUser = await _repository.UserRepository.GetByIdAsync(dto.UserId);
            var secondUser = await _repository.UserRepository.GetByIdAsync(dto.FriendId);

            if (firstUser == null) 
            {
                response.ResponseResult = CreateFriendshipResult.UserNotFound; 
                return response;
            }

            if (secondUser == null) 
            {
                response.ResponseResult = CreateFriendshipResult.FriendNotFound;
                return response;
            }

            var friendshipEntity = MapperConfig.InitializeAutomapper().Map<FriendshipEntity>(dto);
            friendshipEntity.SecondUser = secondUser;
            friendshipEntity.FirstUser = firstUser;

            var result = await _repository.FriendshipRepository.CreateAsync(friendshipEntity);

            if (result == null) 
            {
                response.ResponseResult = CreateFriendshipResult.Unsuccess;
                return response;
            }

            response.ResponseResult = CreateFriendshipResult.Success;
            response.Value = MapperConfig.InitializeAutomapper().Map<FriendshipModel>(result);
            return response;
        }

        public async Task<Response<bool, DeleteUserResult>> DeleteUser(DeleteUserDto dto)
        {
            var response = new Response<bool, DeleteUserResult>();
            var user = await _repository.UserRepository.GetByIdAsync(dto.UserId);

            if (user == null) 
            {
                response.ResponseResult = DeleteUserResult.UserNotFound;
                return response;
            }

            var isUserDeleted = await _repository.UserRepository.DeleteAsync(user);

            if (isUserDeleted)
            {
                response.ResponseResult = DeleteUserResult.DatabaseError;
                return response;
            }

            response.Value = isUserDeleted;
            response.ResponseResult = DeleteUserResult.Success;
            return response;
        }

        public async Task<Response<List<UserModel>, GetAllUsers>> GetAll()
        {
            var response = new Response<List<UserModel>, GetAllUsers>();

            var entities = await _repository.UserRepository.GetAll();

            var models = entities
                .Select(x => MapperConfig.InitializeAutomapper().Map<UserModel>(x))
                .ToList();

            foreach (var model in models)
            {
                model.AvatarBytes = await GetUserAvatar(model.Id);
            }

            response.ResponseResult = GetAllUsers.Success;
            response.Value = models;
            return response;
        }

        public async Task<Response<UserModel, GetUserResult>> GetUserById(FindUserDto dto)
        {
            var response = new Response<UserModel, GetUserResult>();

            var entity = await _repository.UserRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                response.ResponseResult = GetUserResult.UserNotFound;
                return response;
            }

            var model = MapperConfig.InitializeAutomapper().Map<UserModel>(entity);
            model.AvatarBytes = await GetUserAvatar(model.Id);

            response.Value = model;
            response.ResponseResult = GetUserResult.Success;
            return response;
        }

        public async Task<Response<List<UserModel>, GetUserFriends>> GetUserFriendsAsync(FindUserDto dto)
        {
            var response = new Response<List<UserModel>, GetUserFriends>();

            var user = await _repository.UserRepository.GetByIdAsync(dto.Id);

            if (user == null) 
            {
                response.ResponseResult = GetUserFriends.UserNotFound;
                return response;
            }

            Expression<Func<FriendshipEntity, bool>> filter = friendship => friendship.FirstUserId == dto.Id || friendship.SecondUserId == dto.Id;

            var friendships = await _repository.FriendshipRepository.GetByFilterAsync(filter);

            if (friendships == null || !friendships.Any())
            {
                response.ResponseResult = GetUserFriends.Success;
                response.Value = new();
                return response;
            }

            var friends = new List<UserModel>();

            foreach (var friend in friendships)
            {
                UserEntity? userEntity;

                if (friend.FirstUserId == dto.Id)
                {
                    userEntity = await _repository.UserRepository.GetByIdAsync(friend.SecondUserId);
                }
                else 
                {
                    userEntity = await _repository.UserRepository.GetByIdAsync(friend.FirstUserId);
                }

                if (userEntity != null) 
                {
                    friends.Add(MapperConfig.InitializeAutomapper().Map<UserModel>(userEntity));
                }
            }

            if (!friends.Any()) 
            {
                response.ResponseResult = GetUserFriends.Success;
                response.Value = new();
                return response;
            }

            foreach (var friend in friends)
            {
                friend.AvatarBytes = await GetUserAvatar(friend.Id);
            }

            response.ResponseResult = GetUserFriends.Success;
            response.Value = friends;
            return response;
        }

        public async Task<Response<UserAuthModel, LoginResult>> Login(LoginDto dto)
        {
            var response = new Response<UserAuthModel, LoginResult>()
            {
                Value = new UserAuthModel()
            };

            Expression<Func<UserEntity, bool>> predicate = user => user.Email == dto.Email;

            var entity = await _repository.UserRepository.GetByPredicateAsync(predicate);

            if (entity != null)
            {
                var role = await _repository.RoleRepository.GetByIdAsync(entity.RoleId);
                
                if (role != null)
                {
                    var hash = dto.Password.GetHash();

                    if (hash.Equals(entity.Password))
                    {
                        var token = GenerateJWT(entity, role.Name);
                        response.Value.Token = token;
                        var model = MapperConfig.InitializeAutomapper().Map<UserModel>(entity);
                        model.AvatarBytes = await GetUserAvatar(model.Id);
                        response.Value.User = model;
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

            //TODO create all method into transaction 

            var response = new Response<bool, RegisterResult>();

            Expression<Func<UserEntity, bool>> userPredicate = user => user.Email == dto.Email;

            var existUser = await _repository.UserRepository.GetByPredicateAsync(userPredicate);

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
            Expression<Func<RoleEntity, bool>> predicate = role => role.Name == "User";

            var role = await _repository.RoleRepository.GetByPredicateAsync(predicate);

            role ??= await _repository.RoleRepository.CreateAsync(new RoleEntity { Name = "User" });

            user.RoleId = role.Id;

            var createdUser = await _repository.UserRepository.CreateAsync(user);

            if (createdUser == null)
            {
                response.ResponseResult = RegisterResult.DataBaseError;
                response.Messages = new List<string>()
                {
                    "Writing to the database isn't successful"
                };
                return response;
            }

            // default chat for every user
            var savedMessages = new ChatEntity 
            {
                Title = "Saved Messages",
                Type = ChatType.SavedMessages,
                Users = { createdUser }
            };

            var createdChat = await _repository.ChatRepository.CreateAsync(savedMessages);

            if (createdChat == null)
            {
                response.ResponseResult = RegisterResult.DataBaseError;
                return response;
            }

            if (dto.File == null)
            {
                response.Value = true;
                response.ResponseResult = RegisterResult.Success;
                return response;
            }
            else 
            {
                var client = new HttpClient();
                var form = new MultipartFormDataContent();

                using (var stream = ((FormFile)dto.File).OpenReadStream())
                {
                    var fileBytes = new byte[stream.Length];
                    await stream.ReadAsync(fileBytes);
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(dto.File.ContentType);
                    form.Add(fileContent, "File", dto.File.FileName);
                }

                form.Add(new StringContent(createdUser.Id.ToString()), "OwnerId");
                form.Add(new StringContent(ImageType.Profile.ToString()), "Type");

                var clientResponse = await client.PostAsync("https://localhost:7159/api/Image/create", form);
                var imageResponse = await clientResponse.Content.ReadFromJsonAsync<Response<ImageModel, CreateImageResult>>();

                Console.WriteLine(imageResponse);
            }

            return response;
        }

        public Task<Response<UserModel, UpdateUserResult>> UpdateUser(UpdateUserDto dto)
        {
            throw new NotImplementedException();
        }

        public Response<string, TokenValidationResult> ValidateToken(TokenDto dto)
        {
            var response = new Response<string, TokenValidationResult>();

            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(dto.Token))
            {
                var jwtSecurityToken = handler.ReadJwtToken(dto.Token);
                var tokenExp = jwtSecurityToken.Claims.First(x => x.Type.Equals("exp")).Value;
                var ticks = long.Parse(tokenExp);
                var tokenDate = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;

                if (tokenDate >= DateTime.UtcNow)
                {
                    response.ResponseResult = TokenValidationResult.Success;
                }
                else 
                {
                    response.ResponseResult = TokenValidationResult.TimeExpired;
                }

                response.Value = dto.Token;
                return response;
            }

            response.ResponseResult = TokenValidationResult.IncorrectToken;
            return response;
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

        private async Task<byte[]?> GetUserAvatar(int id) 
        {
            if (await _repository.UserRepository.GetByIdAsync(id) == null)
            {
                return null;
            }
            Expression<Func<ImageEntity, bool>> predicate = image => image.OwnerId == id && image.Type == ImageType.Profile;

            var image = await _repository.ImageRepository.GetByPredicateAsync(predicate);

            if (image == null) 
            {
                return null;
            }

            var imageBytes = await File.ReadAllBytesAsync(image.Path);

            return imageBytes;
        }
    }
}
