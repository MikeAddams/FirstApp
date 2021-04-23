using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public List<Category> GetAllCategories();
    }
}
