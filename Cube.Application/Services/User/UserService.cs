using Cube.Application.Services.User.Dto;
using Cube.Application.Utilities;
using Cube.Core.Models.User;
using Cube.EntityFramework.Repository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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

        public async Task<Response<UserEntity, CreateUserResult>> CreateUser(NewUserDto dto)
        {
            // account validation

            var response = new Response<UserEntity, CreateUserResult>();

            var account = await _repository.AccountRepository.GetAccountById(dto.AccountId);

            if (account == null)
            {
                response.ResponseResult = CreateUserResult.AccountNotFound;
                response.Messages = new List<string>()
                {
                    $"Account doesn't exist"
                };
                return response;
            }

            var isAnyRelationToAccount = _repository
                .UserRepository
                .UserAssociatedWithTheAccount(dto.AccountId);

            if (isAnyRelationToAccount != null)
            {
                response.ResponseResult = CreateUserResult.AccountLinkError;
                response.Messages = new List<string>()
                {
                    $"Some user already links to this account"
                };
                return response;
            }

            // name & surname validation
            if (string.IsNullOrEmpty(dto.Name)
                || string.IsNullOrEmpty(dto.Surname))
            {
                response.ResponseResult = CreateUserResult.ValidationError;
                response.Messages = new List<string>()
                {
                    $"Name or surname is null ro empty"
                };
                return response;
            }

            // birthday validation
            if (dto.DateOfBirth != null
                && dto.DateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                response.ResponseResult = CreateUserResult.DateError;
                response.Messages = new List<string>()
                {
                    "Birthday date cannot be later than the current date"
                };
                return response;
            }

            var user = MapperConfig.InitializeAutomapper().Map<UserEntity>(dto);
            user.Account = account;

            var result = await _repository.UserRepository.CreteUser(user);

            if (result == null)
            {
                response.ResponseResult = CreateUserResult.DataBaseError;
                response.Messages = new List<string>()
                {
                    "Writing to the database isn't successful"
                };
                return response;
            }

            response.ResponseResult = CreateUserResult.Success;
            response.Value = result;
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

            var account = await _repository.AccountRepository.GetAccountByEmail(dto.Email);

            if (account != null)
            {
                var hash = dto.Password.GetHash();

                if (hash.Equals(account.PasswordHash))
                {
                    var token = GenerateJWT(account);
                    response.Value.token = token;
                    response.Value.User = _repository.UserRepository.UserAssociatedWithTheAccount(account.Id);
                    response.ResponseResult = LoginResult.Success;
                }
                else
                {
                    response.ResponseResult = LoginResult.WrongLoginOrPassword;
                }

                return response;
            }

            response.ResponseResult = LoginResult.WrongLoginOrPassword;

            return response;
        }

        public async Task<Response<bool, RegisterResult>> Register(RegisterDto dto)
        {
            var response = new Response<bool, RegisterResult>();
            var existAccount = await _repository.AccountRepository.GetAccountByEmail(dto.Email);

            if (existAccount != null)
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

            var account = MapperConfig.InitializeAutomapper().Map<AccountEntity>(dto);

            var result = await _repository.AccountRepository.CreateAccount(account);

            if (result == null)
            {
                response.ResponseResult = RegisterResult.DataBaseError;
                response.Messages = new List<string>()
                {
                    "Writing to the database isn't successful"
                };
                return response;
            }

            var newUser = MapperConfig.InitializeAutomapper().Map<NewUserDto>(dto);
            newUser.AccountId = result.Id;

            var user = await CreateUser(newUser);

            if (user.ResponseResult != CreateUserResult.Success)
            {
                response.ResponseResult = (RegisterResult)user.ResponseResult;
                response.Messages = user.Messages;
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

        private string GenerateJWT(AccountEntity account)
        {
            var authParams = _authOptions.Value;

            var securityKey = authParams.SummetricKey;
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new(ClaimTypes.Email, account.Email),
                new(ClaimTypes.Role, account.Role.ToString()),
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
