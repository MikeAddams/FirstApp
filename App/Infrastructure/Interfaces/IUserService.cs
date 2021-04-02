using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Infrastructure.Interfaces
{
    public interface IUserService
    {
        public Task<UserCredentialsModel> GetUserDetails(string username);
        public Task<UserAuthModel> ValidateUser(LoginModel userModel);
        public Task<UserAuthModel> RegisterUser(RegisterModel userModel);
    }
}
