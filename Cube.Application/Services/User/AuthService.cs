using Cube.Application.Services.User.Dto;
using Cube.Core.Models.User;
using Cube.EntityFramework.Repository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cube.Application.Services.User
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IOptions<AuthOptions> _authOptions;

        public AuthService(IRepositoryWrapper repository, IOptions<AuthOptions> options)
        {
            _repository = repository;
            _authOptions = options;
        }

        public async Task<Response<string, SignInResult>> Login(SignInDto dto)
        {
            var response = new Response<string, SignInResult>();

            var account = await _repository.AccountRepository.GetAccount(dto.Email);

            if (account != null)
            {
                var hash = dto.Password.GetHash();

                if (hash.Equals(account.PasswordHash))
                {
                    var token = GenerateJWT(account);

                    response.Value = token;
                    response.ResponseResult = SignInResult.Success;
                }
                else
                {
                    response.ResponseResult = SignInResult.WrongLoginOrPassword;
                }

                return response;
            }

            response.ResponseResult = SignInResult.WrongLoginOrPassword;

            return response;
        }

        public async Task<Response<AccountEntity, SignUpResult>> Register(SignUpDto dto)
        {
            var response = new Response<AccountEntity, SignUpResult>();
            var existAccount = await _repository.AccountRepository.GetAccount(dto.Email);

            if (existAccount != null)
            {
                response.ResponseResult = SignUpResult.EmailAlreadyExists;
                return response;
            }

            var account = new AccountEntity() 
            {
                Email = dto.Email,
                PasswordHash = dto.Password.GetHash(),

            };

            var result = await _repository.AccountRepository.CreateAccount(account);

            if (result != null) 
            {
                response.ResponseResult = SignUpResult.Success;
                response.Value = result;
                return response;
            }

            return response;
        }

        private string GenerateJWT(AccountEntity account)
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
