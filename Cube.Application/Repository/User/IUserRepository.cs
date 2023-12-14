using Cube.Application.Repository.User.Dto;
    
namespace Cube.Application.Repository.User
{
    public interface IUserRepository
    {
        Response GetUserById(FindUserDto dto);
    }
}
