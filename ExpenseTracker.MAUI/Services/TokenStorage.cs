using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Services
{
    /// <summary>
    /// Stores JWT, refresh tokens and user's id
    /// </summary>
    public class TokenStorage
    {
        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";
        private const string UserIdKey = "userId";

        public static async Task SaveAccessTokenAsync(string token)
        {
            await SecureStorage.SetAsync(AccessTokenKey, token);
        }

        public static async Task<string?> GetAccessTokenAsync()
        {
            return await SecureStorage.GetAsync(AccessTokenKey);
        }

        public static void RemoveAccessToken()
        {
            SecureStorage.Remove(AccessTokenKey);
        }

        public static async Task SaveRefreshTokenAsync(string token)
        {
            await SecureStorage.SetAsync(RefreshTokenKey, token);
        }

        public static async Task<string?> GetRefreshTokenAsync()
        {
            return await SecureStorage.GetAsync(RefreshTokenKey);
        }

        public static void RemoveRefreshToken()
        {
            SecureStorage.Remove(RefreshTokenKey);
        }

        public static async Task SaveUserIdAsync(string userId)
        {
            await SecureStorage.SetAsync(UserIdKey, userId);
        }

        public static async Task<string?> GetUserIdAsync()
        {
            return await SecureStorage.GetAsync(UserIdKey);
        }

        public static void RemoveUserId()
        {
            SecureStorage.Remove(UserIdKey);
        }

        public static void ClearAll()
        {
            RemoveRefreshToken();
            RemoveUserId();
        }
    }
}
