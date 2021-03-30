using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class ProductShowcaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }

        public string ThumbNailPath { get; set; }
    }
}
