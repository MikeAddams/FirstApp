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
using System.Threading.Tasks;

namespace App.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext Data;

        public AccountController(AppDbContext context)
        {
            Data = context;
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
        public IActionResult Profile()
        {
            //User user = Data.Users.FirstOrDefault(x => x.Username == username);

            //string username = User.Identity.Name;
            //string userId = User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            Int32.TryParse(User.Claims.FirstOrDefault(x => x.Type == "Id").Value, out int userId);

            User user = Data.Users.FirstOrDefault(x => x.Id == userId);

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
                User user = await Data.Users.FirstOrDefaultAsync(x => x.Username == model.Username && x.Password == model.Password);

                if (user != null)
                {
                    await Authenticate(user.Id.ToString(), model.Username);

                    return Redirect("/Account/Profile");
                }
                ModelState.AddModelError("", "Incorrect username or password");
            }

            return View(model); //??
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await Data.Users.FirstOrDefaultAsync(x => x.Username == model.Username);

                if (user == null)
                {
                    Data.Users.Add(new User
                    {
                        Username = model.Username,
                        Email = model.Email,
                        Nickname = model.Nickname,
                        Password = model.Password,
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    });
                    await Data.SaveChangesAsync();

                    string lastId = Data.Users.MaxAsync(x => x.Id).ToString();
                    await Authenticate(lastId, model.Username);

                    return Redirect("/Account/Profile");
                }
                else
                    ModelState.AddModelError("", "Username is already in use");
            }

            return View(model); //??
        }

        private async Task Authenticate(string Id, string username)
        {
            var claims = new List<Claim>()
            {
                new Claim("Id", Id),
                new Claim(ClaimTypes.Name, username)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
    }
}
