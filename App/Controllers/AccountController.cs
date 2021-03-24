using App.Models;
using Data;
using Managers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManager UserManager;

        public AccountController(IUserManager userManager)
        {
            UserManager = userManager;
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
            var user = await UserManager.GetByUsername(User.Identity.Name);

            return View(user);
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
                var userEntity = new User
                {
                    Username = model.Username,
                    Password = model.Password
                };

                var user = await UserManager.CheckUserCredentials(userEntity);

                if (user != null)
                {
                    await Authenticate(user.Username, user.Role);

                    return Redirect("/Account/Profile");
                }
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
                var usernameAvaible = await UserManager.CheckIfUsernameAvaible(model.Username);

                if (usernameAvaible)
                {
                    var newUser = new User
                    {
                        Username = model.Username,
                        Email = model.Email,
                        Nickname = model.Nickname,
                        Password = model.Password,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Role = RoleType.Client
                    };

                    await UserManager.RegisterUser(newUser);
                    await Authenticate(model.Username, RoleType.Client);

                    return Redirect("/Account/Profile");
                }
                else
                    ModelState.AddModelError("", "Username is already in use");
            }

            return View();
        }

        private async Task Authenticate(string username, RoleType role)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
    }
}
