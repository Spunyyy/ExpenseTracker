using ExpenseTracker.API.Data;
using ExpenseTracker.API.Entities;
using ExpenseTracker.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController(AppDbContext context) : ControllerBase
    {
        /// <summary>
        /// Add new expense to user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Expense>> AddExpense(ExpenseDTO request)
        {
            if (request == null || request.Date == null || string.IsNullOrWhiteSpace(request.Category) || request.Amount <= 0)
            {
                return BadRequest("Invalid data.");
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Expense expense = new Expense(request, userId);

            context.Expenses.Add(expense);
            await context.SaveChangesAsync();

            return Ok(expense);
        }

        /// <summary>
        /// Deletes expense by id if it belongs to user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var expense = context.Expenses.FirstOrDefault(e => e.Id == id && e.UserId == userId);
            if(expense == null)
            {
                return NotFound("Expense not found.");
            }

            context.Remove(expense);
            await context.SaveChangesAsync();

            return Ok("Success.");
        }

        /// <summary>
        /// Edits existing user expense by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Expense>> EditExpense(int id, ExpenseDTO request)
        {
            if(id <= 0)
            {
                return BadRequest("Invalid ID.");
            }
            if(request == null || request.Date == null || string.IsNullOrWhiteSpace(request.Category) || request.Amount <= 0)
            {
                return BadRequest("Invalid data.");
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var expense = context.Expenses.FirstOrDefault(e => e.Id == id && e.UserId == userId);
            if (expense == null)
            {
                return NotFound("Expense not found.");
            }

            expense.CopyFromDTO(request);
            await context.SaveChangesAsync();

            return Ok(expense);
        }

        /// <summary>
        /// Gets list of all user expenses, can be filtered by year and month.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Expense>> GetExpenses([FromQuery] int? year = null, int? month = null)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var expenses = context.Expenses.Where(e => e.UserId == userId);

            if (year != null)
            {
                expenses = expenses.Where(e => e.Date.Year == year);
            }
            if(month != null)
            {
                if(year == null)
                {
                    expenses = expenses.Where(e => e.Date.Year == DateTime.Now.Year);
                }
                expenses = expenses.Where(e => e.Date.Month == month);
            }

            return Ok(expenses.OrderBy(e => e.Id));
        }

        /// <summary>
        /// Get expense by id if it belongs to user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Expense> GetExpense(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Invalid id.");
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var expense = context.Expenses.FirstOrDefault(e => e.Id == id && e.UserId == userId);

            if(expense == null)
            {
                return NotFound("Expense not found.");
            }

            return Ok(expense);
        }
    }
}
