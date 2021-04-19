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

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Exit()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            int userId = int.Parse(User.FindFirstValue("Id"));

            EditProductModel prodModel = await prodService.GetEditProductModel(id, userId);

            if (prodModel == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Home");
            }

            return View(prodModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductModel updatedModel)
        {
            var userId = int.Parse(User.FindFirstValue("Id"));
            var productResult =await prodService.UpdateProduct(updatedModel, userId);

            if (productResult.IsSuccessful)
            {
                TempData["ProductStatus"] = "Product was updated succesfully";
                return RedirectToAction("EditProduct", "Manager");
            }

            TempData["ProductStatus"] = productResult.Message;
            return RedirectToAction("EditProduct", "Manager");
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

        [HttpPost]
        public async Task<IActionResult> SaveNewProduct(AddProductModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ProductStatus"] = "Invalid fields";
                return RedirectToAction("AddProduct");
            }

            int managerId = int.Parse(User.FindFirstValue("Id"));
            var productResult = await prodService.AddProduct(model, managerId);

            if (!productResult.IsSuccessful)
            {
                TempData["ProductStatus"] = productResult.Message;
                return RedirectToAction("AddProduct");
            }

            TempData["ProductStatus"] = productResult.Message;
            return RedirectToAction("AddProduct");
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


/*
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductModel model)
        {
            return Execute(() =>
            {
                int managerId = int.Parse(User.FindFirstValue("Id"));

                await prodService.AddProduct(model, managerId);
            }, RedirectToAction("AddProduct", "Manager"), RedirectToAction("AddProduct", "Manager"));

            if (ModelState.IsValid)
            {
                
            }

            return RedirectToAction("AddProduct", "Manager");
        } 
         

        public ActionResult Execute(Action action, ActionResult success, ActionResult error)
        {
            if (ModelState.IsValid)
            {
                action();

                
                return success;
            }

            return error;
        }
        */
