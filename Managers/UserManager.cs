using Data;
using Repositories;
using System.Threading.Tasks;

namespace Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository userRepo;

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
            //await userRepo.Users.AddAsync(newUser);
            await userRepo.Add(newUser);
            await userRepo.Commit();

            return newUser;
        }

    }
}
