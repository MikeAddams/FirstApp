using Data;
using Managers.Exceptions;
using Managers.Interfaces;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository userRepo;

        public bool IsUsernameAvaible = false;

        public UserManager(IUserRepository _userRepo)
        {
            userRepo = _userRepo;
        }

        public async Task<User> GetByUsername(string username)
        {
            if (username == null || username.Length == 0)
                throw new InvalidUsernameException("empty");

            if (username.Length > 25 || username.Length < 3)
                throw new InvalidUsernameException("length does not correspond");

            return await userRepo.GetByUsername(username);
        }

        public async Task<User> GetByUserId(int userId)
        {
            return await userRepo.GetById(userId);
        }

        public async Task<User> CheckUserCredentials(User passedUser)
        {
            var user = await userRepo.GetByUsername(passedUser.Username);

            if (user == null)
                throw new InvalidUserException("incorrect user credentials");

            var hashedPassword = HashPassword(passedUser.Password);

            if (user.Password != hashedPassword)
                throw new InvalidUserException("incorrect user credentials");

            return user;
        }

        public async Task<int> RegisterUser(User user)
        {
            if (!IsUsernameAvaible)
            {
                return 0;
            }

            var hashedPassword = HashPassword(user.Password);
            user.Password = hashedPassword;

            await userRepo.Add(user);
            await userRepo.Commit();

            int userId = await userRepo.GetLastUserId();

            return userId;
        }

        public async Task<bool> CheckIfUsernameAvaible(string username)
        {
            var user = await GetByUsername(username);

            if (user != null)
            {
                return false;
            }

            IsUsernameAvaible = true;

            return true;
        }


        private static string HashPassword(string password, string algorithm = "sha256")
        {
            return Hash(Encoding.UTF8.GetBytes(password), algorithm);
        }

        private static string Hash(byte[] input, string algorithm)
        {
            using (var hashAlgorithm = HashAlgorithm.Create(algorithm))
            {
                return Convert.ToBase64String(hashAlgorithm.ComputeHash(input));
            }
        }
    }
}
