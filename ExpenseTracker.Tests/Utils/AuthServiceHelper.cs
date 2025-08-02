using ExpenseTracker.API.Data;
using ExpenseTracker.API.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests.Utils
{
    public class AuthServiceHelper
    {
        public static AuthService GetAuthService(AppDbContext context)
        {
            return new AuthService(context, new ConfigurationBuilder().AddInMemoryCollection( new Dictionary<string, string>
            {
                {"AppSettings:Token", "testSecretKey123456789012345678901234567890123456789012345678901" },
                {"AppSettings:Issuer", "testIssuer" },
                {"AppSettings:Audience", "testAudience" }
            }).Build());
        }
    }
}
