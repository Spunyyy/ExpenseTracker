using ExpenseTracker.API.Data;
using ExpenseTracker.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController(AppDbContext context) : ControllerBase
    {
        /// <summary>
        /// Gets list of summarized expenses by month.
        /// </summary>
        /// <returns></returns>
        [HttpGet("monthly")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MonthlyExpenseDTO>> GetMonthlyExpenses()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var expenses = context.Expenses.Where(e => e.UserId == userId)
                                                    .GroupBy(e => new { e.Date.Year, e.Date.Month })
                                                    .Select(g => new MonthlyExpenseDTO
                                                    {
                                                        Year = g.Key.Year,
                                                        Month = g.Key.Month,
                                                        TotalAmount = g.Sum(x => x.Amount)
                                                    })
                                                    .OrderBy(x => x.Year)
                                                    .ThenBy(x => x.Month)
                                                    .ToList();
            return Ok(expenses);
        }

        /// <summary>
        /// Gets list of summarized expenses by category.
        /// </summary>
        /// <returns></returns>
        [HttpGet("category")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MonthlyExpenseDTO>> GetCategoryExpenses()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var expenses = context.Expenses.Where(e => e.UserId == userId)
                                                    .GroupBy(e => new { e.Category })
                                                    .Select(g => new CategoryExpenseDTO
                                                    {
                                                        Category = g.Key.Category,
                                                        TotalAmount = g.Sum(x => x.Amount)
                                                    })
                                                    .OrderByDescending(x => x.TotalAmount)
                                                    .ToList();
            return Ok(expenses);
        }
    }
}
