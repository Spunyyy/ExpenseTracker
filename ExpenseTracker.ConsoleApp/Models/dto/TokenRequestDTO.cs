using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ConsoleApp.Models.dto
{
    internal class TokenRequestDTO
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
