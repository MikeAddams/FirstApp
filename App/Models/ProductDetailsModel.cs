using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class ProductDetailsModel
    {
        public string Titile { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public Image Image { get; set; }
    }
}