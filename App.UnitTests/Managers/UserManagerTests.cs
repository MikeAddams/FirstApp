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

        [Fact]
        public async Task RegisterUser_CallsCheckIfUsernameAvaibleOnce_If_IsUsernameAvaibleEqualNull()
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();
            User notFoundUser = null;
            var user = new User
            {
                Username = "validUsername",
                FirstName = "ValidFN",
                LastName = "ValidLN",
                Nickname = "ValidNKM",
                Email = "valid@mail.com",
                Password = "validPass"
            };

            userRepoMock.Setup(x => x.GetByUsername(user.Username))
                .ReturnsAsync(notFoundUser);

            var userManager = new UserManager(userRepoMock.Object);

            // Act
            await userManager.RegisterUser(user);

            // Assert
            userRepoMock.Verify(x => x.GetByUsername(user.Username), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RegisterUser_ReturnsUserIdIfUsernameIsAvaible_Else_ThrowsInvalidUserException(bool _isUsernameAvaibale)
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();
            int expectedUserId = 5;
            var passedUser = new User
            {
                FirstName = "ValidFN",
                LastName = "ValidLN",
                Nickname = "ValidNKM",
                Email = "valid@mail.com",
                Password = "validPass"
            };

            userRepoMock.Setup(x => x.GetLastUserId())
                .ReturnsAsync(expectedUserId);

            var userManager = new UserManager(userRepoMock.Object)
            {
                IsUsernameAvaible = _isUsernameAvaibale
            };

            // Act & Assert
            if (_isUsernameAvaibale)
            {
                var result = await userManager.RegisterUser(passedUser);
                Assert.Equal(result, expectedUserId);
            }
            else
            {
                await Assert.ThrowsAsync<InvalidUserException>(
                    async () => await userManager.RegisterUser(passedUser));
            }
        }

        [Theory]
        [InlineData("iii", "iii", "iii", "iii")]
        [InlineData("ValidFN", "iii", "iii", "iii")]
        [InlineData("ValidFN", "ValidLN", "iii", "iii")]
        [InlineData("ValidFN", "ValidLN", "ValidNKM", "ii")]
        [InlineData("invalidFirstNameMoreThan25", "ValidLN", "ValidNKM", "valid@mail.com")]
        [InlineData("ValidFN", "invalidLastNameMoreThan25Char", "ValidNKM", "valid@mail.com")]
        [InlineData("ValidFN", "ValidLN", "invalidNickNameMoreThan25", "valid@mail.com")]
        [InlineData("ValidFN", "ValidLN", "ValidNKM", "invalidEmail")]
        public async Task RegisterUser_ThrowsInvalidUserException_If_DoesNotPassValidation(
            string _firstName, string _lastName, string _nickname, string _email)
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();
            int expectedUserId = 5;

            var passedUser = new User
            {
                FirstName = _firstName,
                LastName = _lastName,
                Nickname = _nickname,
                Email = _email,
                Password = "validPass"
            };

            userRepoMock.Setup(x => x.GetLastUserId())
                .ReturnsAsync(expectedUserId);

            var userManager = new UserManager(userRepoMock.Object)
            {
                IsUsernameAvaible = true
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUserException>(
                    async () => await userManager.RegisterUser(passedUser));
        }

        [Fact]
        public async Task CheckIfUsernameAvaible_RetursFalseANDIsUsernameAvaibleSetedToFalse_When_UsernameIsAlreadyUsed()
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();

            userRepoMock.Setup(x => x.GetByUsername(It.IsAny<string>()))
                .ReturnsAsync(new User());

            var userManager = new UserManager(userRepoMock.Object);

            // Act
            var result = await userManager.CheckIfUsernameAvaible("validUsername");
            var isUsernameAvaible = userManager.IsUsernameAvaible;

            // Assert
            Assert.False(result);
            Assert.False(isUsernameAvaible);
        }
    }
}
