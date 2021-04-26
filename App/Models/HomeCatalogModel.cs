using System.Collections.Generic;

namespace App.Models
{
    public class HomeCatalogModel
    {
        public List<ProductShowcaseModel> Products { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }
}
