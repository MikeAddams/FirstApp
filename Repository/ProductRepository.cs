using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly RepoDbContext Db;

        public ProductRepository(RepoDbContext db)
        {
            Db = db;
        }

        public async Task<Product> GetById(int id)
        {
            return await Db.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public List<Product> GetLast(int count)
        {
            List<Product> ListOfProducts = Db.Products.OrderByDescending(p => p.Id).Take(count).ToList();

            return ListOfProducts;
        }
    }
}
