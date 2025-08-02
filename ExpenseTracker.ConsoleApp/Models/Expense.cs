using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ConsoleApp.Models
{
    internal class Expense
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

        public Expense(int amount, string category, DateTime date, string note)
        {
            Amount = amount;
            Category = category;
            Date = date;
            Note = note;
        }

        public void Update(string amount, string category, string date, string note)
        {
            Amount = string.IsNullOrEmpty(amount) ? Amount : int.Parse(amount);
            Category = string.IsNullOrEmpty(category) ? Category : category;
            Date = string.IsNullOrEmpty(date) ? Date : DateTime.SpecifyKind(DateTime.Parse(date), DateTimeKind.Utc);
            Note = string.IsNullOrEmpty(amount) ? Note : note;
        }
    }
}
