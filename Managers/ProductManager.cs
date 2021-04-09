using Data;
using Managers.Interfaces;
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

        public async Task<Product> GetProductById(int productId)
        {
            return await prodRepo.GetById(productId);
        }

        public async Task<Product> GetProduct(int productId, int managerId)
        {
            var product = await prodRepo.GetById(productId);

            if (product == null || product.ManagerId != managerId)
            {
                return null;
            }

            return product;
        }

        public async Task<string> AddNewProduct(Product prod)
        {
            await prodRepo.Add(prod);
            await prodRepo.Commit();

            return ""; //
        }

        public async Task DeleteProduct(int productId)
        {
            prodRepo.Delete(productId);
            await prodRepo.Commit();
        }

        public async Task UpdateProduct(Product updatedProduct, int managerId)
        {
            if (updatedProduct.ManagerId == managerId)
            {
                prodRepo.Update(updatedProduct);
                await prodRepo.Commit();
            }
        }

        public List<Product> GetLastProducts(int count)
        {
            return prodRepo.GetLast(count);
        }

        public List<Product> GetProductsByManagerId(int managerId)
        {
            return prodRepo.GetProductsByManagerId(managerId);
        }

    }
}
