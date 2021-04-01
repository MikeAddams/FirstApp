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
            var joinedData = await Db.Products
                .Join(
                    Db.Images,
                    prod => prod.ThumbNailId,
                    thumb => thumb.Id,
                    (prod, thumb) => new
                    {
                        Id = prod.Id,
                        Name = prod.Name,
                        Details = prod.Details,
                        Price = prod.Price,
                        Thumbnail = thumb.ThumbNailPath,
                        FullSize = thumb.FullSizePath
                    }
                ).FirstOrDefaultAsync(x => x.Id == id);

            
            var product = new Product
            {
                Id = id,
                Name = joinedData.Name,
                Details = joinedData.Details,
                Price = joinedData.Price,
                ThumbNail = new Image
                {
                    ThumbNailPath = joinedData.Thumbnail,
                    FullSizePath = joinedData.FullSize
                }
            };

            return product;
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
                        Id = prod.Id,
                        Name = prod.Name,
                        Price = prod.Price,
                        thumbPath = thumb.ThumbNailPath
                    }
                ).Take(count).ToList();

            List<Product> ListOfProducts = new List<Product>();

            foreach (var data in joinedData)
            {
                ListOfProducts.Add(new Product
                {
                    Id = data.Id,
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
