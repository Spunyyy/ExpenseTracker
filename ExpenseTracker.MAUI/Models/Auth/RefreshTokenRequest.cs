using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Models.Auth
{
    public class RefreshTokenRequest
    {
        public required Guid UserId { get; set; }
        public required string RefreshToken { get; set; } = string.Empty;
    }
}
