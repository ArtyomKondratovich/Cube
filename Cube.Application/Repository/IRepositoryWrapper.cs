using Cube.Application.Repository.Message;
using Cube.Application.Repository.User;

namespace Cube.Application.Repository
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IMessageRepository Message { get; }
        void Save();
    }
}
