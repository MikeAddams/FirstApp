using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class AddProductModel
    {
        [Required(ErrorMessage = "There's no product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "There's no product price")]
        public float Price { get; set; }
        [Required(ErrorMessage = "There's no product description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Thumbnail is required")]
        public IFormFile ThumbNail { get; set; }
        [Required(ErrorMessage = "Full size image is required")]
        public IFormFile FullSize { get; set; }
    }
}
