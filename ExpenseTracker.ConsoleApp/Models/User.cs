using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ConsoleApp.Models
{
    internal class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;

    }
}
