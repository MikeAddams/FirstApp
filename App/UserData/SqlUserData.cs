using App.Models;
using App.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace App.UserData
{
    public class SqlUserData : IUserData
    {
        private readonly AppDbContext Db;

        public SqlUserData(AppDbContext db)
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

        public async Task<int> Commit()
        {
            return await Db.SaveChangesAsync();
        }
    }
}
