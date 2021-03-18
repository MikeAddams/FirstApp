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
    }
}
