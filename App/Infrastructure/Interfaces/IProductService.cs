using App.Models;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        public Task<ProductDetailsModel> GetProduct(int id);
    }
}
