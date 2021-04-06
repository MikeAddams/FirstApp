using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class ManagerProductsModel
    {
        public List<ProductShowcaseModel> ShowcaseProducts { get; set; }
        public int DeleteProductId { get; set; }
    }
}
