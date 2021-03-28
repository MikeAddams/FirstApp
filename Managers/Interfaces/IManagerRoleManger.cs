using Data;
using System.Threading.Tasks;

namespace Managers.Interfaces
{
    public interface IManagerRoleManger
    {
        public Task<User> ChangeRoleToManager(string username);
    }
}
