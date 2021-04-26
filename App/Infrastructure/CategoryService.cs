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

            var Parents = categoryEntities.Where(x => x.Parent == null);
            var Children = categoryEntities.Where(x => x.Parent != null);

            var childrenList = new List<CategoryModel>();

            foreach (var child in Children)
            {
                childrenList.Add(new CategoryModel
                {
                    ParentId = child.ParentId,
                    Name = child.Name
                });
            }

            foreach (var parent in Parents)
            {
                categoryModels.Add(new CategoryModel
                {
                    Name = parent.Name,
                    Children = childrenList.Where(x => x.ParentId == parent.Id).ToList()
                });
            }

            return categoryModels;
        }
    }
}
