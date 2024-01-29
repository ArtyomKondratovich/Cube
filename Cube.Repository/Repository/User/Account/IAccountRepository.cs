using Cube.Core.Models.User;

namespace Cube.EntityFramework.Repository.User.Account
{
    public interface IAccountRepository
    {
        Task<AccountModel?> GetAccount(string email);
        Task<AccountModel?> CreateAccount(AccountModel model);
    }
}
