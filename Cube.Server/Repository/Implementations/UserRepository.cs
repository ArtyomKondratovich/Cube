using Cube.Server.Models;
using Cube.Server.Models.Dto;
using Cube.Server.Models.ResultObjects;
using Cube.Server.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cube.Server.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly RepositoryContext _contex;
        public UserRepository(RepositoryContext context) 
        {
            _contex = context;
        }

        public Result GetUserById(FindUserDto dto)
        {
            var result = new Result()
            {
                ReturnObject = null,
                ActionResult = new BadRequestResult(),
                Errors = new List<string>()
            };

            if (_contex.Users != null)
            {
                var user = _contex.Users.Find(dto.Id);

                if (user == null)
                {
                    result.Errors.Add("User not found");
                }
                else 
                {
                    result.ReturnObject = user;
                    result.ActionResult = new OkResult();
                }
            }
            else 
            {
                result.Errors.Add("Users not found");
            }

            return result;
        }
    }
}
