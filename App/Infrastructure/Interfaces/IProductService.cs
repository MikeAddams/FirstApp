using App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductCRUDResultModel> GetProductById(int productId);
        public Task<ProductCRUDResultModel> GetProduct(int productId, int managerId);
        public List<ProductShowcaseModel> GetLastProducts(int count);
        public Task<ProductCRUDResultModel> AddProduct(AddProductModel product, int managerId);
        public ManagerProductsModel GetManagerProducts(int id);
        public Task<ProductCRUDResultModel> DeleteProduct(int productId);
        public Task<ProductCRUDResultModel> GetEditProductModel(int productId, int userId);
        public Task<ProductCRUDResultModel> UpdateProduct(EditProductModel updatedProd, int managerId);
        public List<ProductShowcaseModel> GetProductsByCategoriesId(List<int> catIds);
    }
}
