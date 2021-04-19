using Data;
using Microsoft.EntityFrameworkCore;
using Repositories.DataContext;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RepoDbContext Db;

        public UserRepository(RepoDbContext db)
        {
            Db = db;
        }

        public async Task<User> GetByUsername(string username)
        {
            return await Db.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> Add(User newUser)
        {
            await Db.Users.AddAsync(newUser);

            return newUser;
        }

        public User Update(User updatedUser)
        {
            var entity = Db.Users.Attach(updatedUser);
            entity.State = EntityState.Modified;

            return updatedUser;
        }

        public async Task<int> GetLastUserId()
        {
            return await Db.Users.MaxAsync(x => x.Id);
        }

        public async Task<User> GetById(int userId)
        {
            return await Db.Users.FindAsync(userId);
        }

        public async Task<int> Commit()
        {
            return await Db.SaveChangesAsync();
        }
    }
}