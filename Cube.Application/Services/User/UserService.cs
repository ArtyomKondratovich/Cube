using Cube.Application.Services.User.Dto;
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

        public async Task<Response<string, LoginResult>> Login(LoginDto dto)
        {
            var response = new Response<string, LoginResult>();

            var account = await _repository.AccountRepository.GetAccount(dto.Email);

            if (account != null)
            {
                var hash = dto.Password.GetHash();

                if (hash.Equals(account.PasswordHash))
                {
                    var token = GenerateJWT(account);

                    response.Value = token;
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

        public async Task<Response<AccountModel, RegisterResult>> Register(RegisterDto dto)
        {
            var response = new Response<AccountModel, RegisterResult>();
            var existAccount = await _repository.AccountRepository.GetAccount(dto.Email);

            if (existAccount != null)
            {
                response.ResponseResult = RegisterResult.EmailAlreadyExists;
                return response;
            }

            var account = new AccountModel() 
            {
                Email = dto.Email,
                PasswordHash = dto.Password.GetHash(),

            };

            var result = await _repository.AccountRepository.CreateAccount(account);

            if (result != null) 
            {
                response.ResponseResult = RegisterResult.Success;
                response.Value = result;
                return response;
            }

            return response;
        }

        private string GenerateJWT(AccountModel account)
        {
            var authParams = _authOptions.Value;

            var securityKey = authParams.SummetricKey;
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>() 
            {
                new(JwtRegisteredClaimNames.Email, account.Email),
                new(JwtRegisteredClaimNames.Sub, account.Id.ToString())
            };

            account.Roles ??= new Role[] { Role.User, Role.Admin };

            foreach (var role in account.Roles) 
            {
                claims.Add(new Claim("role", role.ToString()));
            }

            var token = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims,
                expires:DateTime.UtcNow.AddSeconds(authParams.TokenLifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
