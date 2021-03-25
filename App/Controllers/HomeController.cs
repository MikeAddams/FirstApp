using App.Models;
using Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
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

        public async Task<IActionResult> IndexAsync()
        {
            /*
            List<Product> products = productData.GetLast(6);

            foreach(var item in products)
            {
                item.ThumbNail = await imageData.GetById(item.ThumbNailId);
            }

            return View(products);
            */

            var a = prodManger.GetLastProducts(2);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
