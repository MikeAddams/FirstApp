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

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUsernameException>(
                async() => await userManager.GetByUsername(username));
        }

        [Fact]
        public async Task CheckUserCredentials_ThrowsInvalidUserException_If_UserDoesNotExist()
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();
            User expectedUser = null;

            userRepoMock.Setup(x => x.GetByUsername(It.IsAny<string>()))
                .ReturnsAsync(expectedUser);

            var userManager = new UserManager(userRepoMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUserException>(
                async() => await userManager.CheckUserCredentials(new User()));
        }

        [Fact]
        public async Task CheckUserCredentials_ThrowsInvalidUserException_If_HashedPasswordsDoNotCorrespond()
        {
            var userRepoMock = new Mock<IUserRepository>();
            User expectedUser = new User
            {
                Password = "vYrajswBoLQwoc9vjJnC+kRliR5sR9bE9jL7Z5fX5r8=" // "correctPass"
            };
            User passedUser = new User
            {
                Password = "incorrectPass"
            };

            userRepoMock.Setup(x => x.GetByUsername(It.IsAny<string>()))
                .ReturnsAsync(expectedUser);

            var userManager = new UserManager(userRepoMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUserException>(
                async () => await userManager.CheckUserCredentials(passedUser));
        }

        [Fact]
        public async Task CheckUserCredentials_ReturnsUser_If_PassesValidation()
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();
            User expectedUser = new User
            {
                Password = "vYrajswBoLQwoc9vjJnC+kRliR5sR9bE9jL7Z5fX5r8=" // "correctPass"
            };
            User passedUser = new User
            {
                Password = "correctPass"
            };

            userRepoMock.Setup(x => x.GetByUsername(It.IsAny<string>()))
                .ReturnsAsync(expectedUser);

            var userManager = new UserManager(userRepoMock.Object);

            // Act
            var result = await userManager.CheckUserCredentials(passedUser);

            // Assert
            Assert.Equal(expectedUser, result);
        }
    }
}
