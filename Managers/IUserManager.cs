using Data;
using System.Threading.Tasks;

namespace Managers
{
    public interface IUserManager
    {
        public Task<User> GetByUsername(string username);
        public Task<User> CheckUserCredentials(User passedUser);
        public Task<bool> CheckIfUsernameAvaible(string username);
        public Task<User> RegisterUser(User user);
    }
}
