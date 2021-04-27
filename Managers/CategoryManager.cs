using Data;
using Managers.Interfaces;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Managers
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository catRepo;

        public CategoryManager(ICategoryRepository _catRepo)
        {
            catRepo = _catRepo;
        }

        public List<Category> GetAll()
        {
            return catRepo.GetAllCategories();
        }

        public Category GetByName(string categoryName)
        {
            var category = catRepo.GetCategoryByName(categoryName);

            if (category == null) new NullReferenceException();

            return category;
        }

        public List<Category> GetAllRelated(int parentId)
        {
            var children = catRepo.GetSubCategories(parentId);
            var categories = new List<Category>();

            foreach (var child in children)
            {
                categories.Add(child);
            }

            return categories;
        }
    }
}
