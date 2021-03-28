using Data;
using Managers.Interfaces;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class ManagerRoleManager : IManagerRoleManger
    {
        private readonly IUserRepository userRepo;

        public ManagerRoleManager(IUserRepository _userRepo)
        {
            userRepo = _userRepo;
        }

        public async Task<User> ChangeRoleToManager(string username)
        {
            var user = await userRepo.GetByUsername(username);

            user.Role = RoleType.Manager;

            userRepo.Update(user);
            await userRepo.Commit();

            return user;
        }
    }
}
