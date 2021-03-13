using App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.UserData
{
    public interface IUserData
    {
        public Task<User> GetByUsername(string username);
        public Task<User> Add(User newUser);

        public Task<int> Commit();
    }
}
