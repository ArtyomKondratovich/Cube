using Cube.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Cube.Application.Repository.User.Dto;

namespace Cube.Application.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly CubeDbContext _contex;
        public UserRepository(CubeDbContext context)
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
