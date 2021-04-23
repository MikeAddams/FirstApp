using App.Infrastructure.Interfaces;
using App.Models;
using Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Infrastructure
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryManager catManager;

        public CategoryService(ICategoryManager _catManager)
        {
            catManager = _catManager;
        }

        public List<CategoryModel> GetAllCategoryModels()
        {
            var categoryEntities = catManager.GetAll();
            var categoryModels = new List<CategoryModel>();

            foreach (var category in categoryEntities)
            {
                categoryModels.Add(new CategoryModel
                {
                    Name = category.Name,
                    ParentId = category.ParentId
                });
            }

            return categoryModels;
        }
    }
}
