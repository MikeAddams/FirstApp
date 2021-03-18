using App.Models;
using App.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Data.Picture
{
    public class SqlImageData : IImageData
    {
        private readonly AppDbContext DB;

        public SqlImageData(AppDbContext db)
        {
            DB = db;
        }

        public async Task<Image> GetById(int id)
        {
            return await DB.Images.FirstOrDefaultAsync(x => x.Id == id);
        }

        public string GetMainThumbNailPath(int id)
        {
            //return DB.Images.FirstOrDefault(x => x.Id == id).ThumbNailPath;
            return "";
        }

        public List<string> GetProductThumbNailsPath(List<Product> products)
        {
            var prodThumbNailPath = new List<string>();

            foreach (var item in products)
            {
                string path = "";
                //var prod = DB.Images.FirstOrDefault(x => x.Id == item.Id);
                string prod = null;

                if (prod != null)
                {
                    //path = prod.ThumbNailPath;
                }
                else
                {
                    path = "http://placehold.it/700x400";
                }

                prodThumbNailPath.Add(path);
            }

            return prodThumbNailPath;
        }
    }
}
