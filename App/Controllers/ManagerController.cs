using App.Models;
using App.UserData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IUserData userData;

        public ManagerController(IUserData user)
        {
            userData = user;
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BecomeManager()
        {
            User user = await userData.GetByUsername(User.Identity.Name);

            user.Role = RoleType.Manager;

            userData.Update(user);
            await userData.Commit();

            return RedirectToAction("Index", "Manager");
        }

    }
}
