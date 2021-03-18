using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Data.Picture
{
    public interface IImageData
    {
        public string GetMainThumbNailPath(int id);
        public List<string> GetProductThumbNailsPath(List<Product> products);

        public Task<Image> GetById(int id);
    }
}
