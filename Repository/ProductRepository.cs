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

        public async Task Delete(int productId)
        {
            var product = await Db.Products
                .Include(x => x.ThumbNail)
                .Where(x => x.Id == productId)
                .FirstAsync();

            var thumb = product.ThumbNail;

            Db.Remove(thumb);
            Db.Remove(product);
            // Idk why it doesn't delete related records 
            // .OnDelete(DeleteBehavior.Cascade) - doesn't work
        }

        public async Task<int> Commit()
        {
            return await Db.SaveChangesAsync();
        }

        public async Task<Product> GetById(int id)
        {
            var product = await Db.Products.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            var thumbNail = await Db.Products
                .Include(x => x.ThumbNail)
                .Select(x => x.ThumbNail)
                .Where(x => x.Id == product.ThumbNailId)
                .FirstAsync();

            product.ThumbNail = thumbNail;

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

        public List<Product> GetProductsByManagerId(int managerId)
        {
            var joinedData = Db.Products
                .Where(x => x.ManagerId == managerId)
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
                ).ToList();

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

        public void Update(Product updatedProduct)
        {
            Db.Products.Update(updatedProduct);
        }

        public List<Product> GetByCategoryId(int categoryId)
        {
            var products = Db.Products.Where(p => p.Category.Id == categoryId).ToList();

            return products;
        }

        public List<Product> GetByCategoriesId(List<int> catIds)
        {
            //var products = Db.Products.Where(p => catIds.Contains(p.Category.Id)).ToList();
            var products = Db.Products
                .Include(p => p.ThumbNail)
                .Where(p => catIds.Contains(p.Category.Id))
                .ToList();

            return products;
        }

    }
}
