using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Services
{
    /// <summary>
    /// Adds stored JWT bearer token to each outgoing HTTP request.
    /// </summary>
    /// <param name="innerHandler"></param>
    public class AuthenticatedHttpHandler(HttpMessageHandler innerHandler) : DelegatingHandler(innerHandler)
    {
        /// <summary>
        /// Gets JWT token from storage and attaches is as Bearer token in the request header if available
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? token = await TokenStorage.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
