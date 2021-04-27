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

        public Category GetCategoryByName(string categoryName)
        {
            return Db.Categories.Where(x => x.Name == categoryName).SingleOrDefault();
        }

        public List<Category> GetSubCategories(int parentId)
        {
            return Db.Categories.Where(c => c.ParentId == parentId).ToList();
        }
    }
}
