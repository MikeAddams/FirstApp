using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ThumbNailPath { get; set; }
        public string FullSizePath { get; set; } 
        public bool IsMainThumbNail { get; set; }
    }
}
