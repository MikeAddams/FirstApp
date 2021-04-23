using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Infrastructure.Interfaces
{
    public interface ICategoryService
    {
        public List<CategoryModel> GetAllCategoryModels();
    }
}
