using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class ProductShowcaseModel
    {
        public string title { get; set; }
        public float Price { get; set; }

        public int ThumbNailId { get; set; }
        public Image ThumbNail { get; set; }
    }
}
