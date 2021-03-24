using App.Models;
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

        //private readonly IProductData productData;
        //private readonly IImageData imageData;

        public HomeController() // IProductData _productData, IImageData _imageData
        {
            //productData = _productData;
            //imageData = _imageData;
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
