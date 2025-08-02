using ExpenseTracker.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Tests.Utils
{
    public class ControllerHelper
    {
        public static async Task<ExpenseController> GetExpenseControllerWithUserAsync()
        {
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new ExpenseController(dbContext);

            var user = dbContext.Users.FirstOrDefault(u => u.Email == "john.doe@example.com");
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = principal }
            };

            return controller;
        }
        public static async Task<StatsController> GetStatsControllerWithUserAsync()
        {
            var dbContext = await DbContextHelper.GetInMemoryDbAsync();
            var controller = new StatsController(dbContext);

            var user = dbContext.Users.FirstOrDefault(u => u.Email == "john.doe@example.com");
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = principal }
            };

            return controller;
        }
    }
}
