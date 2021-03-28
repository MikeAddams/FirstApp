using Data;
using Microsoft.EntityFrameworkCore;
using Repositories.DataContext;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace Repositories
{
    class ImageRepository : IImageRepository
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
    }
}
