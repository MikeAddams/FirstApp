using App.Models;
using Services.Interfaces;
using Managers.Interfaces;
using System.Threading.Tasks;
using Data;
using System.IO;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductManager prodManager;

        public ProductService(IProductManager _prodManager)
        {
            prodManager = _prodManager;
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
