using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Managers.Interfaces
{
    public interface ICategoryManager
    {
        public List<Category> GetAll();
    }
}
