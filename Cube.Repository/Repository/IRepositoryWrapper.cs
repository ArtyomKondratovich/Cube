﻿using Cube.EntityFramework.Repository.Chat;
using Cube.EntityFramework.Repository.Message;
using Cube.EntityFramework.Repository.User;
using Cube.EntityFramework.Repository.User.Account;

namespace Cube.EntityFramework.Repository
{
    public interface IRepositoryWrapper
    {
        public IUserRepository UserRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public IChatRepository ChatRepository { get; }
        public IAccountRepository AccountRepository { get; }
    }
}
