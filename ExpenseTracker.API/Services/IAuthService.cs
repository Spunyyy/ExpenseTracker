using ExpenseTracker.API.Entities;
using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDTO request);
        Task<TokenResponseDTO?> LoginAsync(UserDTO request);
        Task<TokenResponseDTO?> RefreshTokensAsync(RefreshTokenRequestDTO request);
    }
}
