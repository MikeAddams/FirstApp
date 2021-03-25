using Data;
using System.Threading.Tasks;

namespace Managers
{
    public interface IManagerRoleManger
    {
        public Task<User> ChangeRoleToManager(string username);
    }
}
