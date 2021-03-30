using App.Models;
using Managers;
using Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductManager prodManger;

        public HomeController(IProductManager _prodManger)
        {
            prodManger = _prodManger;
        }

        public IActionResult IndexAsync()
        {
            var products = prodManger.GetLastProducts(2);

            var showcaseProds = new List<ProductShowcaseModel>();

            foreach (var prod in products)
            {
                showcaseProds.Add(new ProductShowcaseModel
                {
                    Title = prod.Name,
                    Price = prod.Price,
                    ThumbNailPath = Path.Combine("\\media\\product", prod.ThumbNail.ThumbNailPath)
                });
            }

            return View(showcaseProds);
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
