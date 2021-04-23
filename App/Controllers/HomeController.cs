using App.Infrastructure.Interfaces;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
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

        public IActionResult NotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
