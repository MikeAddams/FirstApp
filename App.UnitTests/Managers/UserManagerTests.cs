using Data;
using Managers;
using Managers.Exceptions;
using Managers.Interfaces;
using Moq;
using NSubstitute;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace App.UnitTests.Managers
{
    public class UserManagerTests
    {
        [Fact]
        public async Task GetByUsername_ShouldReturnUser_On_ValidUsername()
        {
            // Arrange
            string username = "validUsername";
            var userRepoMock = new Mock<IUserRepository>();
            var expectedUser = new User();

            userRepoMock.Setup(x => x.GetByUsername(username))
                .ReturnsAsync(expectedUser);

            var userManager = new UserManager(userRepoMock.Object);

            // Act
            var result = await userManager.GetByUsername(username);

            // Assert
            Assert.Equal(expectedUser, result);
        }

        [Theory]
        [InlineData("invalidUsernameLenghtHigherThan25")]
        [InlineData("ii")]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetByUsername_ThrowsInvalidUsernameException_On_InvalidUsername(string username)
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();
            var expectedUser = new User();

            userRepoMock.Setup(x => x.GetByUsername(username))
                .ReturnsAsync(expectedUser);

            var userManager = new UserManager(userRepoMock.Object);

            // Assert
            await Assert.ThrowsAsync<InvalidUsernameException>(async() => await userManager.GetByUsername(username));
        }
    }
}
