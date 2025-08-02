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
    public class StatsControllerTests
    {
        [Fact]
        public async Task GetMonthlyExpenses_ValidUser_ReturnsOk()
        {
            //Arrange
            var controller = await ControllerHelper.GetStatsControllerWithUserAsync();

            //Act
            var result = controller.GetMonthlyExpenses();

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            var resultValue = objectResult.Value as IEnumerable<MonthlyExpenseDTO>;
            resultValue.Should().NotBeNull();
            resultValue.Count().Should().Be(3);
        }

        [Fact]
        public async Task GetCategoryExpenses_ValidUser_ReturnsOk()
        {
            //Arrange
            var controller = await ControllerHelper.GetStatsControllerWithUserAsync();

            //Act
            var result = controller.GetCategoryExpenses();

            //Assert
            var objectResult = result.Result as OkObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            var resultValue = objectResult.Value as IEnumerable<CategoryExpenseDTO>;
            resultValue.Should().NotBeNull();
            resultValue.Count().Should().Be(4);
        }
    }
}
