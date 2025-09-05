using ExpenseTracker.MAUI.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Services
{
    public class AuthService : ApiService
    {
        /// <summary>
        /// Attempts to log in user using provided credentials.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var response = await client.PostAsJsonAsync("auth/login", request);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var data = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if(data == null)
            {
                return false;
            }

            await SaveTokens(data);

            var responseUserId = await client.GetAsync("user/me");

            if (responseUserId.IsSuccessStatusCode)
            {
                var dataUserId = await responseUserId.Content.ReadFromJsonAsync<UserDetailResponse>();
                await TokenStorage.SaveUserIdAsync(dataUserId.Id.ToString());
            }

            return true;
        }

        /// <summary>
        /// Saves access and refresh tokens from login response to secure storage.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task SaveTokens(LoginResponse response)
        {
            await TokenStorage.SaveAccessTokenAsync(response.AccessToken);
            await TokenStorage.SaveRefreshTokenAsync(response.RefreshToken);
        }

        /// <summary>
        /// Registers new user using provided login request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> RegisterAsync(LoginRequest request)
        {
            var response = await client.PostAsJsonAsync("auth/register", request);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Refreshes authentication tokens by sending refresh token request to server.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>.</returns>
        public async Task<bool> RefreshTokensAsync(RefreshTokenRequest request)
        {
            var response = await client.PostAsJsonAsync("auth/refresh-token", request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var data = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (data == null)
            {
                return false;
            }

            await SaveTokens(data);

            return true;
        }

    }
}
