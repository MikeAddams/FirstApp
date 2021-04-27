using System.Collections.Generic;

namespace App.Models
{
    public class CategoryProductsModel
    {
        public List<ProductShowcaseModel> Products { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public string CurrentCategory { get; set; }
    }
}
