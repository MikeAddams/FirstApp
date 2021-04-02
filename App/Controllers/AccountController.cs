using App.Infrastructure.Interfaces;
using App.Models;
using Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService _userService)
        {
            userService = _userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userModel = await userService.GetUserDetails(User.Identity.Name);

            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userModel = await userService.ValidateUser(model);

                if (userModel != null)
                {
                    await Authenticate(userModel.Id, userModel.Username, userModel.Role);

                    return Redirect("/Account/Profile");
                }
                else
                    ModelState.AddModelError("", "Incorrect username or password");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userModel = await userService.RegisterUser(model);

                if (userModel == null)
                {
                    ModelState.AddModelError("", "Username is already in use");
                } 

                await Authenticate(userModel.Id, userModel.Username, userModel.Role);

                return Redirect("/Account/Profile");
            }

            return View();
        }

        private async Task Authenticate(int id, string username, RoleType role)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim("Id", id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
    }
}
