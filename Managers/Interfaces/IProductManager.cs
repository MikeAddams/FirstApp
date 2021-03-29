using Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Managers.Interfaces
{
    public interface IProductManager
    {
        public List<Product> GetLastProducts(int count);
        public Task<string> AddNewProduct(Product prod);
    }
}
