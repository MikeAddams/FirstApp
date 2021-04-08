using Data;
using Microsoft.EntityFrameworkCore;
using Repositories.DataContext;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly RepoDbContext Db;

        public ImageRepository(RepoDbContext db)
        {
            Db = db;
        }

        public async Task<Image> GetById(int id)
        {
            return await Db.Images.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Image updatedImage)
        {
            Db.Images.Update(updatedImage);
        }

        public async Task<int> Commit()
        {
            return await Db.SaveChangesAsync();
        }
    }
}
