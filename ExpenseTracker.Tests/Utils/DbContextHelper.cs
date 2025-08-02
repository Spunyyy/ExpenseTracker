using ExpenseTracker.API.Data;
using ExpenseTracker.API.Entities;
using ExpenseTracker.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests.Utils
{
    public class DbContextHelper
    {
        public static async Task<AppDbContext> GetInMemoryDbAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureCreated();

            var hasher = new PasswordHasher<User>();

            Dictionary<string, string> users = new Dictionary<string, string>
            {
                { "john.smith@example.com", "JohnSmith123" },
                { "john.doe@example.com", "JohnDoe123" }
            };

            foreach (string email in users.Keys)
            {
                User user = new User(email);
                user.SetPasswordHash(hasher.HashPassword(user, users[email]));
                dbContext.Users.Add(user);
            }

            await dbContext.SaveChangesAsync();


            Dictionary<string, List<ExpenseDTO>> userExpenses = new()
            {
                ["john.smith@example.com"] = new List<ExpenseDTO>
                {
                    /*ID=1*/ new ExpenseDTO { Amount = 10, Category = "Entertainment", Date = DateTime.SpecifyKind(DateTime.Parse("2025-06-27"), DateTimeKind.Utc), Note = "Netflix" },
                    /*ID=2*/ new ExpenseDTO { Amount = 50, Category = "Food", Date = DateTime.SpecifyKind(DateTime.Parse("2025-06-27"), DateTimeKind.Utc) },
                    /*ID=3*/ new ExpenseDTO { Amount = 200, Category = "Electronic", Date = DateTime.SpecifyKind(DateTime.Parse("2025-06-28"), DateTimeKind.Utc) },
                    /*ID=4*/ new ExpenseDTO { Amount = 5, Category = "Entertainment", Date = DateTime.SpecifyKind(DateTime.Parse("2025-06-29"), DateTimeKind.Utc), Note = "Spotify" },
                    /*ID=5*/ new ExpenseDTO { Amount = 25, Category = "Entertainment", Date = DateTime.SpecifyKind(DateTime.Parse("2025-06-30"), DateTimeKind.Utc) },
                    /*ID=6*/ new ExpenseDTO { Amount = 20, Category = "Food", Date = DateTime.SpecifyKind(DateTime.Parse("2025-07-01"), DateTimeKind.Utc) },
                    /*ID=7*/ new ExpenseDTO { Amount = 20, Category = "Transport", Date = DateTime.SpecifyKind(DateTime.Parse("2025-07-01"), DateTimeKind.Utc) },
                    /*ID=8*/ new ExpenseDTO { Amount = 40, Category = "Transport", Date = DateTime.SpecifyKind(DateTime.Parse("2025-07-03"), DateTimeKind.Utc) },
                    /*ID=9*/ new ExpenseDTO { Amount = 10, Category = "Food", Date = DateTime.SpecifyKind(DateTime.Parse("2025-07-03"), DateTimeKind.Utc) }
                },
                ["john.doe@example.com"] = new List<ExpenseDTO>
                {
                    /*ID=10*/ new ExpenseDTO { Amount = 35, Category = "Food", Date = DateTime.SpecifyKind(DateTime.Parse("2024-06-22"), DateTimeKind.Utc) },
                    /*ID=11*/ new ExpenseDTO { Amount = 250, Category = "Electronic", Date = DateTime.SpecifyKind(DateTime.Parse("2025-06-27"), DateTimeKind.Utc) },
                    /*ID=12*/ new ExpenseDTO { Amount = 10, Category = "Transport", Date = DateTime.SpecifyKind(DateTime.Parse("2025-06-28"), DateTimeKind.Utc) },
                    /*ID=13*/ new ExpenseDTO { Amount = 18, Category = "Food", Date = DateTime.SpecifyKind(DateTime.Parse("2025-06-28"), DateTimeKind.Utc) },
                    /*ID=14*/ new ExpenseDTO { Amount = 380, Category = "Electronic", Date = DateTime.SpecifyKind(DateTime.Parse("2025-06-30"), DateTimeKind.Utc) },
                    /*ID=15*/ new ExpenseDTO { Amount = 16, Category = "Entertainment", Date = DateTime.SpecifyKind(DateTime.Parse("2025-06-30"), DateTimeKind.Utc) },
                    /*ID=16*/ new ExpenseDTO { Amount = 20, Category = "Food", Date = DateTime.SpecifyKind(DateTime.Parse("2025-06-30"), DateTimeKind.Utc) },
                    /*ID=17*/ new ExpenseDTO { Amount = 56, Category = "Transport", Date = DateTime.SpecifyKind(DateTime.Parse("2025-07-01"), DateTimeKind.Utc) },
                    /*ID=18*/ new ExpenseDTO { Amount = 48, Category = "Food", Date = DateTime.SpecifyKind(DateTime.Parse("2025-07-02"), DateTimeKind.Utc) },
                    /*ID=19*/ new ExpenseDTO { Amount = 23, Category = "Food", Date = DateTime.SpecifyKind(DateTime.Parse("2025-07-04"), DateTimeKind.Utc) }
                }
            };

            foreach (string email in userExpenses.Keys)
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Email == email);
                foreach (ExpenseDTO expenseDto in userExpenses[email])
                {
                    dbContext.Expenses.Add(new Expense(expenseDto, user.Id));
                }
            }

            await dbContext.SaveChangesAsync();

            return dbContext;
        }

    }
}
