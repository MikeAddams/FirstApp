using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService prodService;

        public ProductController(IProductService _prodService)
        {
            prodService = _prodService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var productResult = await prodService.GetProductById(id);

            if (productResult.IsSuccessful == false)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Home");
            }

            return View(productResult.ProductDetails);
        }
    }
}
