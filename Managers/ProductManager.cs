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

            if (product == null)
                throw new ProductException("Not Found");

            if (product.ManagerId != managerId)
                throw new PermissionException("Not Allowed");

            return product;
        }

        public async Task AddNewProduct(Product prod, RoleType role)
        {
            if (role == RoleType.Client) 
                throw new PermissionException("Not Allowed");

            if (prod.Name.Length > 25 || prod.Name.Length < 5)
                throw new ProductException("Name lenght does not correspond");

            if (prod.Details.Length > 355)
                throw new ProductException("Details lenght does not correspond");

            await prodRepo.Add(prod);
            await prodRepo.Commit();
        }

        public async Task DeleteProduct(int productId)
        {
            await prodRepo.Delete(productId);
            await prodRepo.Commit();
        }

        public async Task UpdateProduct(Product updatedProduct, int managerId)
        {
            if (updatedProduct.ManagerId != managerId) 
                throw new PermissionException("Not Allowed");

            if (updatedProduct.Name.Length > 25 || updatedProduct.Name.Length < 5)
                throw new ProductException("Name lenght does not correspond");

            if (updatedProduct.Details.Length > 355)
                throw new ProductException("Details lenght does not correspond");

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

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            return prodRepo.GetByCategoryId(categoryId);
        }

        public List<Product> GetProductsByCategoriesId(List<int> catIds)
        {
            return prodRepo.GetByCategoriesId(catIds);
        }

    }
}
