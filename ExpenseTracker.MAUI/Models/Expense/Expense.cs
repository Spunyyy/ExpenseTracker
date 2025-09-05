using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Models.Expense
{
    public class Expense
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Note { get; set; }

    }
}
