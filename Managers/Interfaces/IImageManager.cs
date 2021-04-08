using Data;
using System.Threading.Tasks;

namespace Managers.Interfaces
{
    public interface IImageManager
    {
        public Task<Image> GetImageById(int id);
        public Task UpdateImage(Image updatedImage);
    }
}
