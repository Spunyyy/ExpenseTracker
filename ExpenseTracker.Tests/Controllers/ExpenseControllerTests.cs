using ExpenseTracker.API.Entities;
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
    public class ExpenseControllerTests
    {
        [Fact]
        public async Task AddExpense_ValidTokenAndRequest_ReturnsOk()
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act
            var result = await controller.AddExpense(new ExpenseDTO { Amount = 10, Category = "Food", Date = DateTime.UtcNow });

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Theory]
        [InlineData(null, 10, "Food")]               //Invalid date
        [InlineData("2025-07-28", -1, "Electronic")] //Negative amount
        [InlineData("2025-07-28", 15, "")]           //Empty category
        public async Task AddExpense_InvalidData_ReturnsBadRequest(string? date, int amount, string category)
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            var dto = new ExpenseDTO
            {
                Date = date != null ? DateTime.Parse(date) : null,
                Amount = amount,
                Category = category
            };

            //Act
            var result = await controller.AddExpense(dto);

            //Assert
            var objectResult = result.Result as BadRequestObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task DeleteExpense_ValidId_ReturnsOk()
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act
            var result = await controller.DeleteExpense(12);

            //Assert
            var objectResult = result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task DeleteExpense_InvalidId_ReturnsBadRequest()
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act
            var result = await controller.DeleteExpense(-5);

            //Assert
            var objectResult = result as BadRequestObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(100)]
        public async Task DeleteExpense_InvalidOrNonOwnedId_ReturnsNotFound(int id)
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act
            var result = await controller.DeleteExpense(id);

            //Assert
            var objectResult = result as NotFoundObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task EditExpense_ValidInput_ReturnsOk()
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act  
            var result = await controller.EditExpense(13, new ExpenseDTO { Amount = 20, Category = "Food", Date = DateTime.UtcNow.AddDays(-6) });

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(100)]
        public async Task EditExpense_InvalidOrNonOwnedId_ReturnsNotFound(int id)
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            var dto = new ExpenseDTO
            {
                Amount = 50,
                Date = DateTime.UtcNow,
                Category = "Electronic"
            };

            //Act
            var result = await controller.EditExpense(id, dto);

            //Assert
            var objectResult = result.Result as NotFoundObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory]
        [InlineData(-5, "2025-07-28", 15, "Food")]        //Invalid id
        [InlineData(13, null, 15, "Food")]                //Invalid date
        [InlineData(13, "2025-07-28", -10, "Electronic")] //Negative amount
        [InlineData(13, "2025-07-28", 50, "")]            //Empty category
        public async Task EditExpense_InvalidData_ReturnsBadRequest(int id, string? date, int amount, string category)
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            var dto = new ExpenseDTO
            {
                Date = date != null ? DateTime.Parse(date) : null,
                Amount = amount,
                Category = category
            };

            //Act
            var result = await controller.EditExpense(id, dto);

            //Assert
            var objectResult = result.Result as BadRequestObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task GetExpense_ValidId_ReturnsOk()
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act
            var result = controller.GetExpense(13);

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetExpense_InvalidId_ReturnsBadRequest()
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act
            var result = controller.GetExpense(-5);

            //Assert
            var objectResult = result.Result as BadRequestObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(100)]
        public async Task GetExpense_InvalidOrNonOwnedId_ReturnsNotFound(int id)
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act
            var result = controller.GetExpense(id);

            //Assert
            var objectResult = result.Result as NotFoundObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetExpenses_WithoutFilter_ReturnsAll()
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act
            var result = controller.GetExpenses();

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            var resultValue = objectResult.Value as IEnumerable<Expense>;
            resultValue.Should().NotBeNull();
            resultValue.Count().Should().Be(10);
        }

        [Fact]
        public async Task GetExpenses_FilterByYear_ReturnsOk()
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act
            var result = controller.GetExpenses(year: 2025);

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            var resultValue = objectResult.Value as IEnumerable<Expense>;
            resultValue.Should().NotBeNull();
            resultValue.Count().Should().Be(9);
        }

        [Fact]
        public async Task GetExpenses_FilterByYearAndMonth_ReturnsOk()
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act
            var result = controller.GetExpenses(year: 2025, month: 6);

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            var resultValue = objectResult.Value as IEnumerable<Expense>;
            resultValue.Should().NotBeNull();
            resultValue.Count().Should().Be(6);
        }

        [Fact]
        public async Task GetExpenses_FilterByMonth_ReturnsOk()
        {
            //Arrange
            var controller = await ControllerHelper.GetExpenseControllerWithUserAsync();

            //Act
            var result = controller.GetExpenses(month: 7);

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            var resultValue = objectResult.Value as IEnumerable<Expense>;
            resultValue.Should().NotBeNull();
            resultValue.Count().Should().Be(3);
        }
    }
}
