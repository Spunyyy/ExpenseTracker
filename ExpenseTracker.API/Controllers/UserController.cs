using ExpenseTracker.API.Data;
using ExpenseTracker.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(AppDbContext context) : ControllerBase
    {
        /// <summary>
        /// Gets user details.
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDetailDTO> GetUserInfo()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userIdStr == null || !Guid.TryParse(userIdStr, out Guid userId))
            {
                return Unauthorized("Invalid user.");
            }

            var user = context.Users.FirstOrDefault(u => u.Id == userId);

            if(user == null)
            {
                return NotFound("User not found.");
            }

            var dto = new UserDetailDTO(user);

            return Ok(dto);
        }

    }
}
