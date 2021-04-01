using App.Models;
using Services.Interfaces;
using Managers.Interfaces;
using System.Threading.Tasks;
using Data;
using System.IO;
using System.Collections.Generic;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductManager prodManager;

        public ProductService(IProductManager _prodManager)
        {
            prodManager = _prodManager;
        }

        public List<ProductShowcaseModel> GetLastProducts(int count)
        {
            List<Product> prodEntity = prodManager.GetLastProducts(count);

            var prodModel = new List<ProductShowcaseModel>();

            foreach (var prod in prodEntity)
            {
                prodModel.Add(new ProductShowcaseModel
                {
                    Id = prod.Id,
                    Title = prod.Name,
                    Price = prod.Price,
                    ThumbNailPath = Path.Combine("\\media\\product", prod.ThumbNail.ThumbNailPath)
                });
            }

            return prodModel;
        }

        public async Task<ProductDetailsModel> GetProduct(int id)
        {
            Product prodEntity = await prodManager.GetProductById(id);

            if (prodEntity == null)
            {
                return null;
            }

            var prodModel = new ProductDetailsModel
            {
                Titile = prodEntity.Name,
                Description = prodEntity.Details,
                Price = prodEntity.Price,
                Image = new Image
                {
                    ThumbNailPath = "",
                    FullSizePath = Path.Combine("\\media\\product", prodEntity.ThumbNail.FullSizePath),
                }
            };

            return prodModel;
        }
    }
}
