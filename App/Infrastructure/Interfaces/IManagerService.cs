using App.Models;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Infrastructure.Interfaces
{
    public interface IManagerService
    {
        public Task<bool> ChangeRoleToManager(string username);
    }
}
