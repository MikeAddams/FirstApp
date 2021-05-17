using App.Models;
using Data;
using Managers.Exceptions;
using Managers.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace App.UnitTests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task AddProduct_ReturnsProductCRUDResultModelWithIsSuccessfulTrue_If_NoExceptionsCatched()
        {
            // Arrange
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(x => x.ValidateFile(It.IsAny<IFormFile>()))
                .Returns(It.IsAny<string>());

            var userManager = new Mock<IUserManager>();
            userManager.Setup(x => x.GetByUserId(It.IsAny<int>()))
                .ReturnsAsync(new User());

            var productService = new ProductServiceBuilder()
                .WithFileManager(fileManager.Object)
                .WithUserManager(userManager.Object)
                .Build();

            // Assert
            var result = await productService.AddProduct(new AddProductModel(), It.IsAny<int>());

            // Act
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public async Task AddProduct_ReturnsProductCRUDResultModelWithIsSuccessfulFalse_On_InvalidImageException()
        {
            // Arrange
            var expectedException = new InvalidImageException("msg");

            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(x => x.ValidateFile(It.IsAny<IFormFile>()))
                .Throws(expectedException);

            var productService = new ProductServiceBuilder()
                .WithFileManager(fileManager.Object)
                .Build();

            // Assert
            var result = await productService.AddProduct(new AddProductModel(), It.IsAny<int>());

            // Act
            Assert.False(result.IsSuccessful);
            Assert.Contains(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task AddProduct_ReturnsProductCRUDResultModelWithIsSuccessfulFalse_On_PermissionException()
        {
            // Arrange
            var expectedException = new PermissionException("msg");

            var userManager = new Mock<IUserManager>();
            userManager.Setup(x => x.GetByUserId(It.IsAny<int>()))
                .ReturnsAsync(new User());

            var prodManager = new Mock<IProductManager>();
            prodManager.Setup(x => x.AddNewProduct(It.IsAny<Product>(), It.IsAny<RoleType>()))
                .ThrowsAsync(expectedException);

            var productService = new ProductServiceBuilder()
                .WithUserManager(userManager.Object)
                .WithProductManager(prodManager.Object)
                .Build();

            // Assert
            var result = await productService.AddProduct(new AddProductModel(), It.IsAny<int>());

            // Act
            Assert.False(result.IsSuccessful);
            Assert.Contains(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task AddProduct_ReturnsProductCRUDResultModelWithIsSuccessfulFalse_On_ProductException()
        {
            // Arrange
            var expectedException = new ProductException("msg");

            var userManager = new Mock<IUserManager>();
            userManager.Setup(x => x.GetByUserId(It.IsAny<int>()))
                .ReturnsAsync(new User());

            var prodManager = new Mock<IProductManager>();
            prodManager.Setup(x => x.AddNewProduct(It.IsAny<Product>(), It.IsAny<RoleType>()))
                .ThrowsAsync(expectedException);

            var productService = new ProductServiceBuilder()
                .WithUserManager(userManager.Object)
                .WithProductManager(prodManager.Object)
                .Build();

            // Assert
            var result = await productService.AddProduct(new AddProductModel(), It.IsAny<int>());

            // Act
            Assert.False(result.IsSuccessful);
            Assert.Contains(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task AddProduct_ReturnsProductCRUDResultModelWithIsSuccessfulFalse_On_InvalidUserException()
        {
            // Arrange
            var expectedException = new InvalidUserException("msg");

            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(x => x.ValidateFile(It.IsAny<IFormFile>()))
                .Returns(It.IsAny<string>());

            var userManager = new Mock<IUserManager>();
            userManager.Setup(x => x.GetByUserId(It.IsAny<int>()))
                .ThrowsAsync(expectedException);

            var productService = new ProductServiceBuilder()
                .WithFileManager(fileManager.Object)
                .WithUserManager(userManager.Object)
                .Build();

            // Assert
            var result = await productService.AddProduct(new AddProductModel(), It.IsAny<int>());

            // Act
            Assert.False(result.IsSuccessful);
            Assert.Contains(expectedException.Message, result.Message);
        }

        [Fact]
        public async Task AddProduct_CallsValidateFileAndUploadFileTwoTimes()
        {
            // Arrange
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(x => x.ValidateFile(It.IsAny<IFormFile>()))
                .Returns(It.IsAny<string>());

            var userManager = new Mock<IUserManager>();
            userManager.Setup(x => x.GetByUserId(It.IsAny<int>()))
                .ReturnsAsync(new User());

            var productService = new ProductServiceBuilder()
                .WithFileManager(fileManager.Object)
                .WithUserManager(userManager.Object)
                .Build();

            // Assert
            await productService.AddProduct(new AddProductModel(), It.IsAny<int>());

            // Act
            fileManager.Verify(x => x.ValidateFile(It.IsAny<IFormFile>()), Times.Exactly(2));
            fileManager.Verify(x => x.UploadFile(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task AddProduct_CallsAddNewProductOnce()
        {
            // Arrange
            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(x => x.ValidateFile(It.IsAny<IFormFile>()))
                .Returns(It.IsAny<string>());

            var userManager = new Mock<IUserManager>();
            userManager.Setup(x => x.GetByUserId(It.IsAny<int>()))
                .ReturnsAsync(new User());

            var prodManager = new Mock<IProductManager>();

            var productService = new ProductServiceBuilder()
                .WithFileManager(fileManager.Object)
                .WithUserManager(userManager.Object)
                .WithProductManager(prodManager.Object)
                .Build();

            // Assert
            await productService.AddProduct(new AddProductModel(), It.IsAny<int>());

            // Act
            prodManager.Verify(x => x.AddNewProduct(It.IsAny<Product>(), It.IsAny<RoleType>()), Times.Once);
        }
    }
}
