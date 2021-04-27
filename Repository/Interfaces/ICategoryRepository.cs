using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public List<Category> GetAllCategories();
        public Category GetCategoryByName(string categoryName);
        public List<Category> GetSubCategories(int parentId);
    }
}
