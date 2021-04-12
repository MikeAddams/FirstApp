using Data;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IImageRepository
    {
        public Task<Image> GetById(int id);

        public void Update(Image updatedImage);
        public void Delete(int imageId);
        public Task<int> Commit();
    }
}
