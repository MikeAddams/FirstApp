using Data;
using Managers.Interfaces;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class ProductManager : IProductManager
    {
        private readonly IProductRepository prodRepo;

        public ProductManager(IProductRepository _prodRepo)
        {
            prodRepo = _prodRepo;
        }

        public async void AddNewProduct(Product prod)
        {
            await prodRepo.Add(prod);
            //await prodRepo.Commit();
        }

        public List<Product> GetLastProducts(int count)
        {
            return prodRepo.GetLast(count);
        }

    }
}
