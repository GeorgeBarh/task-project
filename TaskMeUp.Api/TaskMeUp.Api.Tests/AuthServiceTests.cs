using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using TaskMeUp.Api.Contracts.DTO;
using TaskMeUp.Api.Entities;
using TaskMeUp.Api.Interfaces.Repositories;
using TaskMeUp.Api.Services;
using Task = System.Threading.Tasks.Task;

namespace TaskMeUp.Api.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            IConfiguration config = new ConfigurationBuilder()
                .AddIniFile("appsettings.ini", optional: true, reloadOnChange: true)
                .Build();


            _authService = new AuthService(config, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Register_ShouldCallDatabaseCorrectly()
        {
            // Arrange
            var userDto = new UserDto { Username = "testuser", Password = "password123" };
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(userDto.Username)).ReturnsAsync((User?)null);
            _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(new User { Username = userDto.Username });

            // Act
            var result = await _authService.Register(userDto);

            // Assert
            Assert.True(result.Success);
            _userRepositoryMock.Verify(repo => repo.GetUserByUsernameAsync(userDto.Username), Times.Once);
            _userRepositoryMock.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Login_ShouldCallDatabaseCorrectly()
        {
            // Arrange
            var userDto = new PartialUserDto { Username = "testuser", Password = "password123" };
            var user = new User { Username = userDto.Username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password) };
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(userDto.Username)).ReturnsAsync(user);

            // Act
            var result = await _authService.Login(userDto);

            // Assert
            Assert.True(result.Success);
            _userRepositoryMock.Verify(repo => repo.GetUserByUsernameAsync(userDto.Username), Times.Once);
        }

        [Fact]
        public async Task GetUserByUsername_ShouldCallDatabaseCorrectly()
        {
            // Arrange
            var username = "testuser";
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, username) }));
            var user = new User { Username = username };
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username)).ReturnsAsync(user);

            // Act
            var result = await _authService.GetUserByUsername(userPrincipal);

            // Assert
            Assert.True(result.Success);
            _userRepositoryMock.Verify(repo => repo.GetUserByUsernameAsync(username), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_ShouldCallDatabaseCorrectly()
        {
            // Arrange
            var oldUsername = "olduser";
            var oldPassword = "olduser";
            var userDto = new UserDto { Username = "newuser", Password = "newpassword" };
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, oldUsername) }));
            var user = new User { Username = oldUsername, PasswordHash = BCrypt.Net.BCrypt.HashPassword(oldPassword) };
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(oldUsername)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _authService.UpdateUser(userPrincipal, userDto, oldPassword);

            // Assert
            Assert.True(result.Success);
            _userRepositoryMock.Verify(repo => repo.GetUserByUsernameAsync(oldUsername), Times.Once);
            _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_ShouldCallDatabaseCorrectly()
        {
            // Arrange
            var username = "testuser";
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, username) }));
            var user = new User { Username = username };
            _userRepositoryMock.Setup(repo => repo.GetUserByUsernameAsync(username)).ReturnsAsync(user);

            // Act
            var result = await _authService.DeleteUser(userPrincipal);

            // Assert
            Assert.True(result.Success);
            _userRepositoryMock.Verify(repo => repo.GetUserByUsernameAsync(username), Times.Once);
            _userRepositoryMock.Verify(repo => repo.DeleteUserAsync(user.Id), Times.Once);
        }
    }
}
