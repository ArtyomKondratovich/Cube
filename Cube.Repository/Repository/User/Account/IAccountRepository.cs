using Cube.Core.Models.User;

namespace Cube.EntityFramework.Repository.User.Account
{
    public interface IAccountRepository
    {
        Task<AccountEntity?> GetAccountById(int id);
        Task<AccountEntity?> GetAccountByEmail(string email);
        Task<AccountEntity?> CreateAccount(AccountEntity model);
    }
}
