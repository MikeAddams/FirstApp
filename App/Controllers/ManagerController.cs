using App.Models;
using App.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class ManagerController : Controller
    {
        //private readonly AppDbContext

        public ManagerController()
        {

        }

        [Authorize(Roles = "Manager")]
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> BecomeManager()
        {

            return View();
        }

    }
}
