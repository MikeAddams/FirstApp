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
        private readonly IUserService userService;

        public ManagerController(IProductService _prodService, IUserService _userService)
        {
            prodService = _prodService;
            userService = _userService;
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

            var prodResult = await prodService.GetEditProductModel(id, userId);

            if (prodResult.IsSuccessful == false)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Home");
            }

            return View(prodResult.EditProductDetails);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductModel updatedModel)
        {
            var userId = int.Parse(User.FindFirstValue("Id"));
            var productResult =await prodService.UpdateProduct(updatedModel, userId);

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
        public async Task<IActionResult> SaveNewProduct(AddProductModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ProductStatus"] = "Invalid fields";
                return RedirectToAction("AddProduct");
            }

            int managerId = int.Parse(User.FindFirstValue("Id"));
            var productResult = await prodService.AddProduct(model, managerId);

            TempData["ProductStatus"] = productResult.Message;
            return RedirectToAction("AddProduct");
        }

        public async Task<IActionResult> BecomeManager()
        {
            var result = await userService.ChangeRoleToManager(User.Identity.Name);

            TempData["userStatus"] = result.Message;
            TempData["userStatusMessage"] = result.Message;

            // need to reAuthentificate somehow
            return RedirectToAction("Profile", "Account");
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<IActionResult> DeleteProduct(ManagerProductsModel manProdModel)
        {
            var productId = manProdModel.DeleteProductId;
            var productResult = await prodService.DeleteProduct(productId);

            TempData["ProductStatus"] = productResult.Message;
            return RedirectToAction("MyProducts", "Manager");
        }

    }
}