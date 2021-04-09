﻿using Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Managers.Interfaces
{
    public interface IProductManager
    {
        public List<Product> GetLastProducts(int count);
        public Task<string> AddNewProduct(Product prod);
        public List<Product> GetProductsByManagerId(int managerId);
        public Task DeleteProduct(int productId);
        public Task UpdateProduct(Product updatedProduct, int managerId);

        public Task<Product> GetProductById(int productId);
        public Task<Product> GetProduct(int productId, int managerId);
    }
}
