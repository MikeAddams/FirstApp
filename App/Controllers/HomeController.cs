using App.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Diagnostics;

namespace App.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductService prodService;

        public HomeController(IProductService _prodService)
        {
            prodService = _prodService;
        }

        public IActionResult IndexAsync()
        {
            var products = prodService.GetLastProducts(10);

            return View(products);
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
