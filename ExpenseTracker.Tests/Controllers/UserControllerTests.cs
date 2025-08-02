using ExpenseTracker.API.Controllers;
using ExpenseTracker.Tests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetUserInfo_ValidUser_ReturnsOk()
        {
            //Arrange
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new UserController(dbContext);

            var user = dbContext.Users.FirstOrDefault(u => u.Email == "john.doe@example.com");
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = principal }
            };

            //Act
            var result = controller.GetUserInfo();

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetUserInfo_InvalidUserIdClaim_ReturnsUnauthorized()
        {
            //Arrange
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new UserController(dbContext);

            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "invalid") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = principal }
            };

            //Act
            var result = controller.GetUserInfo();

            //Assert
            var objectResult = result.Result as UnauthorizedObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task GetUserInfo_UserDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new UserController(dbContext);

            var userId = Guid.NewGuid();
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = principal }
            };

            //Act
            var result = controller.GetUserInfo();

            //Assert
            var objectResult = result.Result as NotFoundObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
