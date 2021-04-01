using App.Models;
using Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IManagerRoleManger manager;

        private readonly IProductService prodService;

        public ManagerController(IManagerRoleManger _manager, IProductService _prodService)
        {
            manager = _manager;

            prodService = _prodService;
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
                int managerId = int.Parse(User.FindFirstValue("Id"));

                await prodService.AddProduct(model, managerId);
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
