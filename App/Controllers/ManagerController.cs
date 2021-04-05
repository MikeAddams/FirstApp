using App.Infrastructure.Interfaces;
using App.Models;
using Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IProductService prodService;
        private readonly IManagerService managerService;

        public ManagerController(IProductService _prodService, IManagerService _managerService)
        {
            prodService = _prodService;
            managerService = _managerService;
        }

        [Authorize(Roles = "Manager,Administrator")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        public IActionResult MyProducts()
        {
            var userId = Int32.Parse(User.FindFirstValue("Id"));
            var productsModel = prodService.GetManagerProducts(userId);

            return View(productsModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductModel model)
        {
            if (ModelState.IsValid)
            {
                int managerId = int.Parse(User.FindFirstValue("Id"));

                await prodService.AddProduct(model, managerId);
            }

            return RedirectToAction("AddProduct", "Manager");
        }

        public async Task<IActionResult> BecomeManager()
        {
            await managerService.ChangeRoleToManager(User.Identity.Name);

            return RedirectToAction("Index", "Manager");
        }

        public IActionResult DeleteProduct(int id)
        {
            prodService.DeleteProduct(id);

            return RedirectToAction("MyProducts", "Manager");
        }

    }
}
