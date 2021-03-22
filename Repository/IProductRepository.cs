using Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IProductRepository
    {
        public Task<Product> GetById(int id);
        public List<Product> GetLast(int count);
    }
}
