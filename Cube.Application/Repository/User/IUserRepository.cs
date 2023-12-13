using Cube.Application.Repository.User.Dto;
    
namespace Cube.Application.Repository.User
{
    public interface IUserRepository
    {
        Result GetUserById(FindUserDto dto);
    }
}
