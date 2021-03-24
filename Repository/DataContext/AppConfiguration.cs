using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.DataContext
{
    public class AppConfiguration
    {
        public string sqlConnection { get; set; }

        public AppConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..\\App\\appsettings.json"); //??

            configBuilder.AddJsonFile(path, false);
            var root = configBuilder.Build();

            var appSettings = root.GetSection("ConnectionStrings:LocalConnection");

            sqlConnection = appSettings.Value;
        }
    }
}
