using Managers.Interfaces;
using Moq;
using Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.UnitTests.Services
{
    public class ProductServiceBuilder
    {
        private IProductManager prodManager;
        private IFileManager fileManager;
        private IUserManager userManager;

        public ProductServiceBuilder()
        {
            prodManager = new Mock<IProductManager>().Object;
            fileManager = new Mock<IFileManager>().Object;
            userManager = new Mock<IUserManager>().Object;
        }

        public ProductServiceBuilder WithProductManager(IProductManager prodManager)
        {
            this.prodManager = prodManager;
            return this;
        }

        public ProductServiceBuilder WithFileManager(IFileManager fileManager)
        {
            this.fileManager = fileManager;
            return this;
        }

        public ProductServiceBuilder WithUserManager(IUserManager userManager)
        {
            this.userManager = userManager;
            return this;
        }

        public ProductService Build()
        {
            return new ProductService(prodManager, fileManager, userManager);
        }
    }
}
