using App.Models;
using App.Models.Context;
using App.Models.View;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _userData;

        public AccountController(AppDbContext userData)
        {
            _userData = userData;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        //[HttpGet("{username}")]
        [Authorize]
        public async Task<IActionResult> Profile(int id)
        {
            string username = User.Identity.Name;

            User user = await _userData.Users.FirstOrDefaultAsync(x => x.Username == username);

            //Int32.TryParse(User.Claims.FirstOrDefault(x => x.Type == "Id").Value, out int userId);
            //User user = Data.Users.FirstOrDefault(x => x.Id == userId);

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
                User user = await _userData.Users.FirstOrDefaultAsync(x => x.Username == model.Username && x.Password == hashedPass);

                if (user != null)
                {
                    await Authenticate(user.Id.ToString(), user.Username, user.Role);

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
                User user = await _userData.Users.FirstOrDefaultAsync(x => x.Username == model.Username);

                if (user == null)
                {
                    _userData.Users.Add(new User
                    {
                        Username = model.Username,
                        Email = model.Email,
                        Nickname = model.Nickname,
                        Password = HashPassword(model.Password),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Role = RoleType.Client
                    });
                    await _userData.SaveChangesAsync();

                    string lastId = _userData.Users.MaxAsync(x => x.Id).ToString();
                    await Authenticate(lastId, model.Username, RoleType.Client);

                    return Redirect("/Account/Profile");
                }
                else
                    ModelState.AddModelError("", "Username is already in use");
            }

            return View(model);
        }

        private async Task Authenticate(string Id, string username, RoleType role)
        {
            var claims = new List<Claim>()
            {
                new Claim("Id", Id),
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

        private static string Hash(byte[] input, string algorithm = "sha256")
        {
            using (var hashAlgorithm = HashAlgorithm.Create(algorithm))
            {
                return Convert.ToBase64String(hashAlgorithm.ComputeHash(input));
            }
        }
    }
}
