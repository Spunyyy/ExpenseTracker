using ExpenseTracker.API.Entities;
using ExpenseTracker.API.Models;
using ExpenseTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Register new user account. Returns error if email already exists.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            var user = await authService.RegisterAsync(request);

            if(user == null)
            {
                return BadRequest("User already exists.");
            }

            return Ok(user);
        }

        /// <summary>
        /// Authenticates user and issues new JWT & refresh tokens.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenResponseDTO>> Login(UserDTO request)
        {
            var result = await authService.LoginAsync(request);

            if(result == null)
            {
                return BadRequest("Invalid email or password.");
            }

            return Ok(result);
        }

        /// <summary>
        /// Issues new JWT & refresh tokens.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO request)
        {
            var result = await authService.RefreshTokensAsync(request);

            if(result == null || result.AccessToken == null || result.RefreshToken == null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            return Ok(result);
        }
    }
}
