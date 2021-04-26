using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public virtual List<CategoryModel> Children { get; set; }
    }
}

/*
 *public int Id { get; set; }
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> Children { get; set; } 
 * */
