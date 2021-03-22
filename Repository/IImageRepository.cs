using Data;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IImageRepository
    {
        public Task<Image> GetById(int id);
    }
}
