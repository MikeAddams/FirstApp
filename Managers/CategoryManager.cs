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
    }
}
