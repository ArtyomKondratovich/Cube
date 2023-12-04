namespace Cube.Server.Repository.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IMessageRepository Message { get; }
        void Save();
    }
}
