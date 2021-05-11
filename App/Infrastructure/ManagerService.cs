using App.Infrastructure.Interfaces;
using Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Infrastructure
{
    public class ManagerService : IManagerService
    {
        private readonly IUserManager userMnager;

        public ManagerService(IUserManager _userMnager)
        {
            userMnager = _userMnager;
        }

    }
}
