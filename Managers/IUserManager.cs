using Data;
using System.Threading.Tasks;

namespace Managers
{
    public interface IUserManager
    {
        public Task<User> GetByUsername(string username);
        public Task<User> Add(User newUser);
    }
}
