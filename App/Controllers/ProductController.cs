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
            var product = await prodService.GetProduct(id);

            if (product == null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(product);
        }
    }
}
