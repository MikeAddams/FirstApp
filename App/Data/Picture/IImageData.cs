using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Data.Picture
{
    public interface IImageData
    {
        public Task<Image> GetById(int id);
    }
}
