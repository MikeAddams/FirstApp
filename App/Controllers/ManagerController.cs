using App.Models;
using Data;
using Managers;
using Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IManagerRoleManger manager;
        private readonly IFileManager fileManager;
        private readonly IProductManager productManager;

        public ManagerController(IManagerRoleManger _manager, IFileManager _fileManager, IProductManager _productManager)
        {
            manager = _manager;
            fileManager = _fileManager;
            productManager = _productManager;
        }

        [Authorize(Roles = "Manager,Administrator")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = fileManager.UploadFile(model.ThumbNail);

                var product = new Product()
                {
                    Name = model.Name,
                    Details = model.Description,
                    Price = model.Price,
                    ThumbNail = new Image
                    {
                        ThumbNailPath = uniqueFileName
                    },
                    //ManagerId
                };

                await productManager.AddNewProduct(product);
            }

            return RedirectToAction("AddProduct", "Manager");
        }

        public async Task<IActionResult> BecomeManager()
        {
            await manager.ChangeRoleToManager(User.Identity.Name);

            return RedirectToAction("Index", "Manager");
        }

    }
}
