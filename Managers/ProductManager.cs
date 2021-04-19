using Data;
using Managers.Exceptions;
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
        private readonly IImageRepository imgRepo;

        public ProductManager(IProductRepository _prodRepo, IImageRepository _imgRepo)
        {
            prodRepo = _prodRepo;
            imgRepo = _imgRepo;
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

        public async Task AddNewProduct(Product prod, RoleType role)
        {
            if (role == RoleType.Client) throw new PermissionException("Adding Product");

            await prodRepo.Add(prod);
            await prodRepo.Commit();
        }

        public async Task DeleteProduct(int productId)
        {
            //var prodDetails = await prodRepo.GetById(productId);
            //var a = prodDetails.

            await prodRepo.Delete(productId);
            await prodRepo.Commit();
        }

        public async Task UpdateProduct(Product updatedProduct, int managerId)
        {
            if (updatedProduct.ManagerId != managerId) throw new PermissionException("Updating Product");

            prodRepo.Update(updatedProduct);
            await prodRepo.Commit();
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
