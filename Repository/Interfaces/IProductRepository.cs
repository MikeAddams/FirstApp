using Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product> GetById(int id);
        public Task<Product> Add(Product newProduct);
        public Task<int> Commit();
        public List<Product> GetLast(int count);
        public List<Product> GetProductsByManagerId(int managerId);
        public Task Delete(int productId);
        public void Update(Product updatedProduct);
        public List<Product> GetByCategoryId(int categoryId);
        public List<Product> GetByCategoriesId(List<int> catIds);
    }
}
