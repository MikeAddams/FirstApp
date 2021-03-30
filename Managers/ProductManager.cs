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

        public async Task<Product> GetProductById(int id)
        {
            return await prodRepo.GetById(id);
        }

        public async Task<string> AddNewProduct(Product prod)
        {
            await prodRepo.Add(prod);
            await prodRepo.Commit();

            return ""; //
        }

        public List<Product> GetLastProducts(int count)
        {
            return prodRepo.GetLast(count);
        }

    }
}
