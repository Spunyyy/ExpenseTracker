using ExpenseTracker.MAUI.Models.Expense;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Services
{
    public class ExpensesService : ApiService
    {
        public ObservableCollection<Expense> Expenses { get; } = new ObservableCollection<Expense>();

        /// <summary>
        /// Adds new expense by sending provided expense data to server.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> AddExpenseAsync(CreateExpense request)
        {
            var response = await client.PostAsJsonAsync("expense", request);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Retrieves list of expenses from server and updates local collection.
        /// </summary>
        /// <returns></returns>
        public async Task GetExpensesAsync()
        {
            Expenses.Clear();

            var response = await client.GetFromJsonAsync<List<Expense>>("expense");

            foreach(var ex in response)
            {
                Expenses.Add(ex);
            }
        }

        /// <summary>
        /// Deletes expense with specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var response = await client.DeleteAsync($"expense/{id}");

            if (response.IsSuccessStatusCode)
            {
                var expense = Expenses.Where(e => e.Id == id).FirstOrDefault();
                Expenses.Remove(expense);
            }
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Retrieves expense by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Expense?> GetExpenseAsync(int id)
        {
            var response = await client.GetFromJsonAsync<Expense?>($"expense/{id}");

            return response;
        }

        /// <summary>
        /// Updates existing expense by sending updated data to the server.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public async Task<bool> UpdateExpenseAsync(Expense ex)
        {
            var response = await client.PutAsJsonAsync($"expense/{ex.Id}", ex);

            return response.IsSuccessStatusCode;
        }
    }
}
