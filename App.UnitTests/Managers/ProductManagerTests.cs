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
    }
}
