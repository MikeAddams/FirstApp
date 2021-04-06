﻿using App.Infrastructure.Interfaces;
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
            var managerProdModel = prodService.GetManagerProducts(userId);

            return View(managerProdModel);
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

        [HttpPost]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<IActionResult> DeleteProduct(ManagerProductsModel manProdModel)
        {
            var productId = manProdModel.DeleteProductId;

            await prodService.DeleteProduct(productId);

            return RedirectToAction("MyProducts", "Manager");
        }
    }
}
