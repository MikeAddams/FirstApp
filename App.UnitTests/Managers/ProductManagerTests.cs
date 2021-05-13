using Data;
using Managers;
using Managers.Exceptions;
using Moq;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace App.UnitTests.Managers
{
    public class ProductManagerTests
    {
        [Fact]
        public async Task GetProduct_ThrowsProductException_If_ProductNotFound()
        {
            // Arrange
            var prodRepoMock = new Mock<IProductRepository>();
            Product expectedProduct = null;

            prodRepoMock.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(expectedProduct);

            var prodManager = new ProductManager(prodRepoMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ProductException>(async () => 
                await prodManager.GetProduct(It.IsAny<int>(), It.IsAny<int>()));
        }

        [Fact]
        public async Task GetProduct_ThrowsPermissionException_If_ManagerIdDoesNotMatch()
        {
            // Arrange
            var prodRepoMock = new Mock<IProductRepository>();
            int passedManagerId = 1;
            Product expectedProduct = new Product() { ManagerId = 2 };

            prodRepoMock.Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(expectedProduct);

            var prodManager = new ProductManager(prodRepoMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<PermissionException>(async () =>
                await prodManager.GetProduct(It.IsAny<int>(), passedManagerId));
        }

        [Fact]
        public async Task AddNewProduct_ThrowsPermissionException_If_IncorrectPermission()
        {
            // Arrange
            var prodManager = new ProductManager(It.IsAny<IProductRepository>());

            // Act & Assert
            await Assert.ThrowsAsync<PermissionException>(async () =>
                await prodManager.AddNewProduct(It.IsAny<Product>(), RoleType.Client));
        }

        [Fact]
        public async Task AddNewProduct_CallsAddAndCommit_If_PassesValidation()
        {
            // Arrange
            var prodRepoMock = new Mock<IProductRepository>();
            var prodManager = new ProductManager(prodRepoMock.Object);
            var product = new Product
            {
                Name = "Product",
                Details = "Product detials"
            };

            // Act
            await prodManager.AddNewProduct(product, RoleType.Manager);

            // Assert
            prodRepoMock.Verify(x => x.Add(It.IsAny<Product>()), Times.Once);
            prodRepoMock.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_CallsDeleteAndCommit()
        {
            // Arrange
            var prodRepoMock = new Mock<IProductRepository>();
            var prodManager = new ProductManager(prodRepoMock.Object);

            // Act
            await prodManager.DeleteProduct(It.IsAny<int>());

            // Assert
            prodRepoMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
            prodRepoMock.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_ThrowsPermissionException_If_ManagerIdsDoesNotMatch()
        {
            // Arrange
            var prodRepoMock = new Mock<IProductRepository>();
            var prodManager = new ProductManager(prodRepoMock.Object);

            int managerId = 1;
            var product = new Product
            {
                ManagerId = 2
            };

            // Act & Assert
            await Assert.ThrowsAsync<PermissionException>(async () =>
                await prodManager.UpdateProduct(product, managerId));
        }

        [Theory]
        [InlineData("prod", "details")]
        [InlineData("productNameThatHasMoreThan25Characters", "details")]
        public async Task UpdateProduct_ThrowsProductException_If_DoesNotPassValidation(
            string prodName, string prodDetials)
        {
            // Arrange
            var prodRepoMock = new Mock<IProductRepository>();
            var prodManager = new ProductManager(prodRepoMock.Object);

            int managerId = 1;
            var product = new Product
            {
                ManagerId = managerId,
                Name = prodName,
                Details = prodDetials
            };

            // Act & Assert
            await Assert.ThrowsAsync<ProductException>(async () =>
                await prodManager.UpdateProduct(product, managerId));
        }

        [Fact]
        public async Task UpdateProduct_CallsUpdateAndCommit_If_IfPassesValidation()
        {
            // Arrange
            var prodRepoMock = new Mock<IProductRepository>();
            var prodManager = new ProductManager(prodRepoMock.Object);

            int managerId = 1;
            var product = new Product
            {
                ManagerId = managerId,
                Name = "product",
                Details = "details"
            };

            // Act
            await prodManager.UpdateProduct(product, managerId);

            // Assert
            prodRepoMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
            prodRepoMock.Verify(x => x.Commit(), Times.Once);
        }
    }
}
