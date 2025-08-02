using ExpenseTracker.API.Controllers;
using ExpenseTracker.API.Models;
using ExpenseTracker.Tests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Register_NewUser_ReturnsOk()
        {
            //Arrange
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new AuthController(AuthServiceHelper.GetAuthService(dbContext));

            //Act
            var result = await controller.Register(new UserDTO { Email = "david.wilson@example.com", Password = "DavidWilson123" });

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Register_UserExists_ReturnsBadRequest()
        {
            //Arrange
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new AuthController(AuthServiceHelper.GetAuthService(dbContext));

            //Act
            var result = await controller.Register(new UserDTO { Email = "john.smith@example.com", Password = "JohnSmith123" });

            //Assert
            var objectResult = result.Result as BadRequestObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Login_RegisteredUser_ReturnsOk()
        {
            //Arrange
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new AuthController(AuthServiceHelper.GetAuthService(dbContext));

            //Act
            var result = await controller.Login(new UserDTO { Email = "john.doe@example.com", Password = "JohnDoe123" });

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact] 
        public async Task Login_InvalidEmail_ReturnsBadRequest()
        {
            //Arrange
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new AuthController(AuthServiceHelper.GetAuthService(dbContext));

            //Act
            var result = await controller.Login(new UserDTO { Email = "invalidUser@example.com", Password = "User123" });

            //Assert
            var objectResult = result.Result as BadRequestObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsBadRequest()
        {
            //Arrange
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new AuthController(AuthServiceHelper.GetAuthService(dbContext));

            //Act
            var result = await controller.Login(new UserDTO { Email = "john.doe@example.com", Password = "User123" });

            //Assert
            var objectResult = result.Result as BadRequestObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task RefreshToken_SuccessRefresh_ReturnsOk()
        {
            //Arrange
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new AuthController(AuthServiceHelper.GetAuthService(dbContext));
            var login = await controller.Login(new UserDTO { Email = "john.doe@example.com", Password = "JohnDoe123" });
            var user = dbContext.Users.FirstOrDefault(u => u.Email == "john.doe@example.com");

            //Act
            var result = await controller.RefreshToken(new RefreshTokenRequestDTO { RefreshToken = user.RefreshToken, UserId = user.Id });

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task RefreshToken_InvalidRefreshToken_ReturnsUnauthorized()
        {
            //Arrange
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new AuthController(AuthServiceHelper.GetAuthService(dbContext));
            var user = dbContext.Users.FirstOrDefault(u => u.Email == "john.doe@example.com");

            //Act
            var result = await controller.RefreshToken(new RefreshTokenRequestDTO { RefreshToken = "", UserId = user.Id });

            //Assert
            var objectResult = result.Result as UnauthorizedObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task RefreshToken_ExpiredToken_ReturnsUnauthorized()
        {
            //Arrange
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new AuthController(AuthServiceHelper.GetAuthService(dbContext));
            var user = dbContext.Users.FirstOrDefault(u => u.Email == "john.doe@example.com");
            user.SetRefreshToken(user.RefreshToken, DateTime.UtcNow.AddDays(-10));
            await dbContext.SaveChangesAsync();

            //Act
            var result = await controller.RefreshToken(new RefreshTokenRequestDTO { RefreshToken = user.RefreshToken, UserId = user.Id });

            //Assert
            var objectResult = result.Result as UnauthorizedObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }
    }
}
