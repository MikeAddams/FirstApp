using App.Infrastructure.Interfaces;
using App.Models;
using Data;
using Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Infrastructure
{
    public class ManagerService : IManagerService
    {
        private readonly IProductManager prodManger;
        private readonly IManagerRoleManger manager;

        public ManagerService(IProductManager _prodManger, IManagerRoleManger _manager)
        {
            prodManger = _prodManger;
            manager = _manager;
        }

        public List<ProductShowcaseModel> GetManagerProducts(int id)
        {
            var prodEntity = prodManger.GetProductsByManagerId(id);
            var prodModel = new List<ProductShowcaseModel>();

            foreach (var prod in prodEntity)
            {
                prodModel.Add(new ProductShowcaseModel
                {
                    Id = prod.Id,
                    Title = prod.Name,
                    Price = prod.Price,
                    ThumbNailPath = prod.ThumbNail.ThumbNailPath
                });
            }

            return prodModel;
        }

        public async Task<bool> ChangeRoleToManager(string username)
        {
            await manager.ChangeRoleToManager(username);

            return true;
        }
    }
}
