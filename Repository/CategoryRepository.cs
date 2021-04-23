using Data;
using Repositories.DataContext;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly RepoDbContext Db;

        public CategoryRepository(RepoDbContext db)
        {
            Db = db;
        }

        public List<Category> GetAllCategories()
        {
            return Db.Categories.ToList();
        }
    }
}
