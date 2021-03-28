using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Managers.Interfaces
{
    public interface IFileManager
    {
        public string UploadFile(IFormFile file);
    }
}
