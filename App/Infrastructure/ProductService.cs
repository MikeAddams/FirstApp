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
        private readonly IFileManager fileManager;

        public ProductService(IProductManager _prodManager, IFileManager _fileManager)
        {
            prodManager = _prodManager;
            fileManager = _fileManager;
        }

        public async Task<bool> AddProduct(AddProductModel prodModel, int managerId)
        {
            string uniqueThumbName = fileManager.UploadFile(prodModel.ThumbNail);
            string uniqueFullsizeName = fileManager.UploadFile(prodModel.FullSize);

            var product = new Product()
            {
                Name = prodModel.Name,
                Details = prodModel.Description,
                Price = prodModel.Price,
                ThumbNail = new Image
                {
                    ThumbNailPath = uniqueThumbName,
                    FullSizePath = uniqueFullsizeName
                },
                ManagerId = managerId
            };

            await prodManager.AddNewProduct(product);

            return true;
        }

        public async Task DeleteProduct(int productId)
        {
            await prodManager.DeleteProduct(productId);
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

        public ManagerProductsModel GetManagerProducts(int id)
        {
            var prodEntity = prodManager.GetProductsByManagerId(id);
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

            var managerProdModel = new ManagerProductsModel
            {
                ShowcaseProducts = prodModel,
                DeleteProductId = -1
            };

            return managerProdModel;
        }
    }
}
