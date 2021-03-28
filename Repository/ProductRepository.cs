using Data;
using Microsoft.EntityFrameworkCore;
using Repositories.DataContext;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Product> Add(Product newProduct)
        {
            await Db.Products.AddAsync(newProduct);

            return newProduct;
        }

        public async Task<int> Commit()
        {
            return await Db.SaveChangesAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await Db.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public List<Product> GetLast(int count)
        {
            var joinedData = Db.Products
                .Join(
                    Db.Images,
                    prod => prod.ThumbNailId,
                    thumb => thumb.Id,
                    (prod, thumb) => new
                    {
                        Name = prod.Name,
                        Price = prod.Price,
                        thumbPath = thumb.ThumbNailPath
                    }
                ).ToList();

            List<Product> ListOfProducts = new List<Product>();

            foreach (var data in joinedData)
            {
                ListOfProducts.Add(new Product
                {
                    Name = data.Name,
                    Price = data.Price,
                    ThumbNail = new Image 
                    { 
                        ThumbNailPath = data.thumbPath 
                    }
                });
            }

            return ListOfProducts;
        }

    }
}
