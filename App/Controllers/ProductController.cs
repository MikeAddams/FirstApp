using App.Models;
using Data;
using Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManager prodManager;

        public ProductController(IProductManager _prodManager)
        {
            prodManager = _prodManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var productEntity = await prodManager.GetProductById(id);

            if (productEntity == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var productModel = new ProductDetailsModel
            {
                Titile = productEntity.Name,
                Description = productEntity.Details,
                Price = productEntity.Price,
                Image = new Image
                {
                    ThumbNailPath = Path.Combine("\\media\\product", productEntity.ThumbNail.ThumbNailPath),
                    FullSizePath = "",
                }
            };

            return View(productModel);
        }
    }
}
