using Data;
using Repositories;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository userRepo;

        private bool IsUsernameAvaible = false;

        public UserManager(IUserRepository _userRepo)
        {
            userRepo = _userRepo;
        }

        public async Task<User> GetByUsername(string username)
        {
            return await userRepo.GetByUsername(username);
        }

        public async Task<User> Add(User newUser)
        {
            await userRepo.Add(newUser);
            await userRepo.Commit();

            return newUser;
        }

        public async Task<User> CheckUserCredentials(User passedUser)
        {
            var user = await userRepo.GetByUsername(passedUser.Username);

            if (user == null)
            {
                return null;
            }

            var hashedPassword = HashPassword(passedUser.Password);

            if (user.Password != hashedPassword)
            {
                return null;
            }

            return user;
        }

        public async Task<User> RegisterUser(User user)
        {
            if (!IsUsernameAvaible)
            {
                return null;
            }

            var hashedPassword = HashPassword(user.Password);
            user.Password = hashedPassword;

            await Add(user);

            return user;
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
