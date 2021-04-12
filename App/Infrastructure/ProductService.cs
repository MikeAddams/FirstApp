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
        private readonly IImageManager imgManager;

        public ProductService(IProductManager _prodManager, IFileManager _fileManager, IImageManager _imgManager)
        {
            prodManager = _prodManager;
            fileManager = _fileManager;
            imgManager = _imgManager;
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

        public async Task<ProductDetailsModel> GetProductById(int productId)
        {
            Product prodEntity = await prodManager.GetProductById(productId);

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

        public async Task<ProductDetailsModel> GetProduct(int productId, int managerId)
        {
            Product prodEntity = await prodManager.GetProduct(productId, managerId);

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

        public async Task<EditProductModel> GetEditProductModel(int productId, int userId)
        {
            Product prodEntity = await prodManager.GetProduct(productId, userId);

            if (prodEntity == null)
            {
                return null;
            }

            var editProdModel = new EditProductModel
            {
                Title = prodEntity.Name,
                Description = prodEntity.Details,
                Price = prodEntity.Price,

                CurrentImage = new Image
                {
                    Id = prodEntity.ThumbNail.Id,
                    ThumbNailPath = Path.Combine("\\media\\product", prodEntity.ThumbNail.ThumbNailPath),
                    FullSizePath = Path.Combine("\\media\\product", prodEntity.ThumbNail.FullSizePath),
                }
            };

            return editProdModel;
        }

        public async Task UpdateProduct(EditProductModel updatedProd, int managerId)
        {
            var prodEntity = await prodManager.GetProductById(updatedProd.Id);

            prodEntity.Name = updatedProd.Title;
            prodEntity.Details = updatedProd.Description;
            prodEntity.Price = updatedProd.Price;

            if (updatedProd.UpdatedThumbNail != null || updatedProd.UpdatedFullSize != null)
            {
                var currentImageEntity = await imgManager.GetImageById(updatedProd.CurrentImage.Id);

                if (updatedProd.UpdatedThumbNail != null)
                {
                    string uniqueThumbName = fileManager.UploadFile(updatedProd.UpdatedThumbNail);
                    
                    fileManager.RemoveFile(currentImageEntity.ThumbNailPath);
                    currentImageEntity.ThumbNailPath = uniqueThumbName;
                }

                if (updatedProd.UpdatedFullSize != null)
                {
                    string uniqueFullsizeName = fileManager.UploadFile(updatedProd.UpdatedFullSize);

                    fileManager.RemoveFile(currentImageEntity.FullSizePath);
                    currentImageEntity.FullSizePath = uniqueFullsizeName;
                }

                //await imgManager.UpdateImage(currentImageEntity);
                prodEntity.ThumbNail = currentImageEntity;
            }

            await prodManager.UpdateProduct(prodEntity, managerId);
        }
    }
}
