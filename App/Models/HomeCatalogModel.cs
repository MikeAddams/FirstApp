using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class HomeCatalogModel
    {
        public List<ProductShowcaseModel> Products { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
}
