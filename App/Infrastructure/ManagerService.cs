using App.Infrastructure.Interfaces;
using Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Infrastructure
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRoleManger manager;

        public ManagerService(IManagerRoleManger _manager)
        {
            manager = _manager;
        }

        public async Task<bool> ChangeRoleToManager(string username)
        {
            await manager.ChangeRoleToManager(username);

            return true;
        }
    }
}
