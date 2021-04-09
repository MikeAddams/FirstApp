using App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDetailsModel> GetProductById(int productId);
        public Task<ProductDetailsModel> GetProduct(int productId, int managerId);
        public List<ProductShowcaseModel> GetLastProducts(int count);
        public Task<bool> AddProduct(AddProductModel product, int managerId);
        public ManagerProductsModel GetManagerProducts(int id);
        public Task DeleteProduct(int productId);
        public Task<EditProductModel> GetEditProductModel(int productId, int userId);
        public Task UpdateProduct(EditProductModel updatedProd, int managerId);
    }
}
