using App.Infrastructure.Interfaces;
using App.Models;
using Data;
using Managers.Interfaces;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Infrastructure
{
    public class UserService : IUserService
    {
        private readonly IUserManager userManager;

        public UserService(IUserManager _userManager)
        {
            userManager = _userManager;
        }

        public async Task<UserCredentialsModel> GetUserDetails(string username)
        {
            var userEntity = await userManager.GetByUsername(username);

            var userModel = new UserCredentialsModel
            {
                Username = userEntity.Username,
                Email = userEntity.Email,
                Nickname = userEntity.Nickname,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Role = userEntity.Role
            };

            return userModel;
        }

        public async Task<UserAuthModel> RegisterUser(RegisterModel userModel)
        {
            var usernameAvaible = await userManager.CheckIfUsernameAvaible(userModel.Username);

            if (usernameAvaible == false)
            {
                return null;
            }

            var userEntity = new User
            {
                Username = userModel.Username,
                Email = userModel.Email,
                Nickname = userModel.Nickname,
                Password = userModel.Password,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Role = RoleType.Client
            };

            int userId = await userManager.RegisterUser(userEntity);

            var userAuthModel = new UserAuthModel
            {
                Id = userId,
                Username = userEntity.Username,
                Role = RoleType.Client
            };

            return userAuthModel;
        }

        public async Task<UserAuthModel> ValidateUser(LoginModel userLoginModel)
        {
            var userEntity = new User
            {
                Username = userLoginModel.Username,
                Password = userLoginModel.Password
            };

            var user = await userManager.CheckUserCredentials(userEntity);

            if (user == null)
            {
                return null;
            }

            var userAuthModel = new UserAuthModel
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };

            return userAuthModel;
        }
    }
}
