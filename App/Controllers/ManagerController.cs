﻿using App.Models;
using Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IManagerRoleManger manager;

        public ManagerController(IManagerRoleManger _manager)
        {
            manager = _manager;
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        public async Task<IActionResult> BecomeManager()
        {
            await manager.ChangeRoleToManager(User.Identity.Name);

            return RedirectToAction("Index", "Manager");
        }

    }
}
