using App.Models;
using App.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.ProductData
{
    public class SqlProductData : IProductData
    {
        private readonly AppDbContext DB;

        public SqlProductData(AppDbContext db)
        {
            DB = db;
        }

        public async Task<Product> GetById(int id)
        {
            return await DB.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public List<Product> GetLast(int count)
        {
            List<Product> ListOfProducts = DB.Products.OrderByDescending(p => p.Id).Take(count).ToList();
            
            return ListOfProducts;
        }

    }
}
