using ExpenseTracker.MAUI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Services
{
    public abstract class ApiService
    {
        protected readonly HttpClient client;

        protected ApiService()
        {
#if DEBUG
            var handler = new AuthenticatedHttpHandler(new InsecureHttpClientHandler());
#else
            var handler = new AuthenticatedHttpHandler(new HttpClientHandler());
#endif
            client = new HttpClient(handler)
            {
                BaseAddress = new Uri(AppConfig.BaseUrl)
            };
        }
    }
}
