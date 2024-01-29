using Cube.Core.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Cube.EntityFramework.Repository.User.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CubeDbContext _dbContext;

        public AccountRepository(CubeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AccountModel?> CreateAccount(AccountModel model)
        {
            var account = await _dbContext.Accounts.AddAsync(model);

            if (account != null)
            {
                await _dbContext.SaveChangesAsync();

                return account.Entity;
            }

            return null;
        }

        public async Task<AccountModel?> GetAccount(string email)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
