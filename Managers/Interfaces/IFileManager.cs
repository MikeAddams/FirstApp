using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Managers.Interfaces
{
    public interface IFileManager
    {
        public string UploadFile(IFormFile file);
        public void UploadFile(IFormFile file, string uniqueFileName);
        public bool RemoveFile(string fileName);
        public string GetUniqueFileName(string currentFileName);
        public string ValidateFile(IFormFile file);
        public bool ReplaceFile(IFormFile file, string fileName);
    }
}
