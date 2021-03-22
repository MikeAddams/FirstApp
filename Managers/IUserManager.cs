using Data;

namespace Managers
{
    public interface IUserManager
    {
        public User GetByUsername(string username);
    }
}
