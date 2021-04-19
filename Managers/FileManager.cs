﻿using Managers.Exceptions;
using Managers.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Managers
{
    public class FileManager : IFileManager
    {
        public bool RemoveFile(string fileName)
        {
            string fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media\\product", fileName);
            FileInfo file = new FileInfo(fullFilePath);

            if (file.Exists)
            {
                file.Delete();
            }
            else
            {
                return false;
            }

            return true;
        }

        public string UploadFile(IFormFile file)
        {
            if (file == null) throw new InvalidImageException("No image found");

            if (file.Length > 500000) throw new InvalidImageException("Image size is too big");

            string uniqueFileName = null;

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media\\product");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public void UploadFile(IFormFile file, string uniqueFileName)
        {
            if (file == null) throw new InvalidImageException("No image found");
            if (file.Length > 500000) throw new InvalidImageException("Image size is too big");

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\media\\product");

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }

        public string GetUniqueFileName(string currentFileName)
        {
            return Guid.NewGuid().ToString() + "_" + currentFileName;
        }
    }
}
