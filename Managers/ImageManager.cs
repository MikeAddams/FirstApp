using Data;
using Managers.Interfaces;
using Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Managers
{
    public class ImageManager : IImageManager
    {
        private readonly IImageRepository imgRepo;

        public ImageManager(IImageRepository _imgRepo)
        {
            imgRepo = _imgRepo;
        }

        public async Task<Image> GetImageById(int id)
        {
            return await imgRepo.GetById(id);
        }

        public async Task UpdateImage(Image updatedImage)
        {
            imgRepo.Update(updatedImage);
            await imgRepo.Commit();
        }
    }
}
