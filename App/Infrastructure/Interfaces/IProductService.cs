using App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDetailsModel> GetProduct(int id);
        public List<ProductShowcaseModel> GetLastProducts(int count);
        public Task<bool> AddProduct(AddProductModel product, int managerId);
        public List<ProductShowcaseModel> GetManagerProducts(int id);
        public void DeleteProduct(int productId);
    }
}
