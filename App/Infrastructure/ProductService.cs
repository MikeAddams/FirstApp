using App.Models;
using Services.Interfaces;
using Managers.Interfaces;
using System.Threading.Tasks;
using Data;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Managers.Exceptions;
using System;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductManager prodManager;
        private readonly IFileManager fileManager;
        private readonly IUserManager userManager;

        public ProductService(IProductManager _prodManager, IFileManager _fileManager, IUserManager _userManager)
        {
            prodManager = _prodManager;
            fileManager = _fileManager;
            userManager = _userManager;
        }

        public async Task<ProductCRUDResultModel> AddProduct(AddProductModel prodModel, int managerId)
        {
            try
            {
                string uniqueThumbName = fileManager.ValidateFile(prodModel.ThumbNail);
                string uniqueFullsizeName = fileManager.ValidateFile(prodModel.FullSize);

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

                var user = await userManager.GetByUserId(managerId);

                await prodManager.AddNewProduct(product, user.Role);

                fileManager.UploadFile(prodModel.ThumbNail, uniqueThumbName);
                fileManager.UploadFile(prodModel.FullSize, uniqueFullsizeName);
            }
            catch (InvalidImageException ex)
            {
                return new ProductCRUDResultModel { IsSuccessful = false, Message = ex.Message };
            }

            return new ProductCRUDResultModel { IsSuccessful = true, Message = "Product saved succesfully!" };
        }

        public async Task<ProductCRUDResultModel> DeleteProduct(int productId)
        {
            try
            {
                var prodDetails = await prodManager.GetProductById(productId);

                var fileNames = new List<string>();
                fileNames.Add(prodDetails.ThumbNail.ThumbNailPath);
                fileNames.Add(prodDetails.ThumbNail.FullSizePath);

                await prodManager.DeleteProduct(productId);

                fileManager.RemoveFiles(fileNames);
            }
            catch (InvalidImageException ex)
            {
                return new ProductCRUDResultModel { IsSuccessful = false, Message = ex.Message };
            }

            return new ProductCRUDResultModel { IsSuccessful = true, Message = "Product removed successfully" };
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

        public async Task<ProductCRUDResultModel> UpdateProduct(EditProductModel updatedProd, int managerId)
        {
            try
            {
                var prodEntity = await prodManager.GetProductById(updatedProd.Id);

                prodEntity.Name = updatedProd.Title;
                prodEntity.Details = updatedProd.Description;
                prodEntity.Price = updatedProd.Price;

                if (updatedProd.UpdatedThumbNail != null)
                {
                    fileManager.ValidateFile(updatedProd.UpdatedThumbNail);
                }
                if (updatedProd.UpdatedFullSize != null)
                {
                    fileManager.ValidateFile(updatedProd.UpdatedFullSize);
                }


                await prodManager.UpdateProduct(prodEntity, managerId);

                fileManager.ReplaceFile(updatedProd.UpdatedThumbNail, prodEntity.ThumbNail.ThumbNailPath);
                fileManager.ReplaceFile(updatedProd.UpdatedFullSize, prodEntity.ThumbNail.FullSizePath);
            }
            catch(PermissionException ex)
            {
                return new ProductCRUDResultModel { IsSuccessful = false, Message = ex.Message };
            }

            return new ProductCRUDResultModel { IsSuccessful = true, Message = "Product updated succesfully!" };
        }

        public List<ProductShowcaseModel> GetProductsByCategoriesId(List<int> catIds)
        {
            var productsEntity = prodManager.GetProductsByCategoriesId(catIds);

            if (productsEntity == null || productsEntity.Count == 0)
            {
                return null;
            }

            var productsModel = new List<ProductShowcaseModel>();

            foreach (var prod in productsEntity)
            {
                productsModel.Add(new ProductShowcaseModel
                {
                    Id = prod.Id,
                    Title = prod.Name,
                    Price = prod.Price,
                    ThumbNailPath = Path.Combine("\\media\\product", prod.ThumbNail.ThumbNailPath)
                });
            }

            return productsModel;
        }
    }
}
