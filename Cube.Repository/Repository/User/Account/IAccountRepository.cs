using Cube.Core.Models.User;

namespace Cube.EntityFramework.Repository.User.Account
{
    public interface IAccountRepository
    {
        Task<AccountEntity?> GetAccount(string email);
        Task<AccountEntity?> CreateAccount(AccountEntity model);
    }
}
