using App.Models;
using App.Models.View;
using App.UserData;
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
        private readonly IUserData UserData;

        public AccountController(IUserData userData)
        {
            UserData = userData;
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
            User user = await UserData.GetByUsername(User.Identity.Name);

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
                string hashedPass = HashPassword(model.Password);
                //User user = await Data.Users.FirstOrDefaultAsync(x => x.Username == model.Username && x.Password == hashedPass);

                User user = await UserData.GetByUsername(model.Username);

                if (user != null && user.Password == hashedPass)
                {
                    await Authenticate(user.Username, user.Role);

                    return Redirect("/Account/Profile");
                }
                ModelState.AddModelError("", "Incorrect username or password");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                //User user = await Data.Users.FirstOrDefaultAsync(x => x.Username == model.Username);
                User user = await UserData.GetByUsername(model.Username);

                if (user == null)
                {
                    var newUser = new User
                    {
                        Username = model.Username,
                        Email = model.Email,
                        Nickname = model.Nickname,
                        Password = HashPassword(model.Password),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Role = RoleType.Client
                    };

                    await UserData.Add(newUser);
                    await UserData.Commit();

                    await Authenticate(model.Username, RoleType.Client);

                    return Redirect("/Account/Profile");
                }
                else
                    ModelState.AddModelError("", "Username is already in use");
            }

            return View(model);
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
