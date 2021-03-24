﻿using Data;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepository
    {
        public Task<User> Add(User newUser);
        public User Update(User updatedUser);

        public Task<User> GetByUsername(string username);

        public Task<int> Commit();
    }
}