using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;


namespace App.ProductData
{
    public interface IProductData
    {
        public Task<Product> GetById(int id);
        public List<Product> GetLast(int count);

    }
}