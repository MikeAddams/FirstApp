using App.Infrastructure.Interfaces;
using App.Models;
using Data;
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

        public int? GetCategoryId(string categoryName)
        {
            int? categoryId;

            try
            {
                var categoryEntity = catManager.GetByName(categoryName);
                categoryId = categoryEntity.Id;
            }
            catch (NullReferenceException)
            {
                categoryId = null;
            }

            return categoryId;
        }

        public List<CategoryModel> GetAllRelatedCategories(int parentId)
        {
            var categoriesEntity = catManager.GetAllRelated(parentId);
            var categroiesModel = new List<CategoryModel>();

            foreach (var cat in categoriesEntity)
            {
                categroiesModel.Add(new CategoryModel
                {
                    Id = cat.Id,
                    Name = cat.Name
                });
            }

            return categroiesModel;
        }

        public List<int> GetAllRelatedCategoriesIds(int parentId)
        {
            var categories = this.GetAllRelatedCategories(parentId);
            var ids = new List<int>();

            foreach (var cat in categories)
            {
                ids.Add(cat.Id);
            }

            return ids;
        }
    }
}
