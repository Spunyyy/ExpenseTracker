using ExpenseTracker.API.Data;
using ExpenseTracker.API.Entities;
using ExpenseTracker.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseTracker.API.Services
{
    public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
    {
        /// <summary>
        /// Authenticates user and issues JWT and refresh token on successful login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TokenResponseDTO?> LoginAsync(UserDTO request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if(user == null)
            {
                return null;
            }

            if(new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        private async Task<TokenResponseDTO> CreateTokenResponse(User user)
        {
            return new TokenResponseDTO { 
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user), 
            };
        }

        /// <summary>
        /// Creates access token containing email and user id
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        /// <summary>
        /// Generate new refresh token and save it
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();

            user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

            await context.SaveChangesAsync();
            return refreshToken;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Issues new JWT and refresh token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TokenResponseDTO?> RefreshTokensAsync(RefreshTokenRequestDTO request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            
            if(user == null)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        /// <summary>
        /// Validate if users refresh token is valid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await context.Users.FindAsync(userId);

            if(user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Register new user account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<User?> RegisterAsync(UserDTO request)
        {
            if(await context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return null;
            }

            var user = new User(request.Email);

            user.SetPasswordHash(new PasswordHasher<User>().HashPassword(user, request.Password));

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }
    }
}
