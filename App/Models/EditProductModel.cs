using Data;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class EditProductModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "There's no product name")]
        public string Title { get; set; }
        [Required(ErrorMessage = "There's no product price")]
        public float Price { get; set; }
        [Required(ErrorMessage = "There's no product description")]
        public string Description { get; set; }

        //public string OldThumbnailPath { get; set; }
        //public string OldFullSizePath { get; set; }

        public Image CurrentImage { get; set; }

        public IFormFile UpdatedThumbNail { get; set; }
        public IFormFile UpdatedFullSize { get; set; }
    }
}
