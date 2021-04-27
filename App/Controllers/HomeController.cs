using App.Infrastructure.Interfaces;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace App.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductService prodService;
        private readonly ICategoryService catService;

        public HomeController(IProductService _prodService, ICategoryService _catService)
        {
            prodService = _prodService;
            catService = _catService;
        }

        public IActionResult IndexAsync()
        {
            var products = prodService.GetLastProducts(10);
            var categories = catService.GetAllCategoryModels();

            var homeModel = new HomeCatalogModel
            {
                Products = products,
                Categories = categories
            };

            return View(homeModel);
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CategoryProducts(string category, string subCategory)
        {
            var categoryId = catService.GetCategoryId(category);

            if (categoryId == null)
            {
                return RedirectToAction("NotFoundPage");
            }

            var subCategoryId = catService.GetCategoryId(subCategory);
            var homeCatalogModel = new CategoryProductsModel();
            var categoriesId = new List<int>();

            if (subCategoryId != null)
            {
                categoriesId.Add((int)subCategoryId);
                homeCatalogModel.CurrentCategory = subCategory;
            }
            else
            {
                categoriesId = catService.GetAllRelatedCategoriesIds((int)categoryId);
                homeCatalogModel.CurrentCategory = category;
            }

            homeCatalogModel.Products = prodService.GetProductsByCategoriesId(categoriesId);
            homeCatalogModel.Categories = catService.GetAllCategoryModels();

            return View(homeCatalogModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
