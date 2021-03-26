using Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public interface IProductManager
    {
        public List<Product> GetLastProducts(int count);
        public void AddNewProduct(Product prod);
    }
}
