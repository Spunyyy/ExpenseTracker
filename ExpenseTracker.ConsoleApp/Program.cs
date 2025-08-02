using AdysTech.CredentialManager;
using ExpenseTracker.ConsoleApp.Models;
using ExpenseTracker.ConsoleApp.Models.dto;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ExpenseTracker.ConsoleApp
{
    internal class Program
    {
        private static HttpClient Client = new HttpClient();

        private static User User;

        private static List<string> Categories = new List<string>
        {
            "Food", "Transport", "Electronic", "Entertainment"
        };

        static async Task Main(string[] args)
        {
            Client.BaseAddress = new Uri("https://localhost:7050/");
            if (!await Login())
            {
                Console.ReadKey();
                return;
            }
            await Menu();
        }

        /// <summary>
        /// Login user to app
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> Login()
        {
            if (await LoginFromToken())
            {
                return true;
            }
            Console.WriteLine("LOGIN");
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            var dto = new UserRequestDTO
            {
                Email = email,
                Password = password
            };

            try
            {
                var response = await Client.PostAsJsonAsync("api/Auth/login", dto);
                return await HandleTokenLoginResponse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Request error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Automatically login user from credentials if anything is saved
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> LoginFromToken()
        {
            var cred = CredentialManager.GetCredentials("ExpenseTracker-App");
            if(cred == null)
            {
                return false;
            }

            var dto = new TokenRequestDTO
            {
                RefreshToken = cred.Password,
                UserId = Guid.Parse(cred.UserName)
            };

            try
            {
                var response = await Client.PostAsJsonAsync("api/Auth/refresh-token", dto);
                return await HandleTokenLoginResponse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Request error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Handles response from login or refresh
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static async Task<bool> HandleTokenLoginResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var tokenDTO = await response.Content.ReadFromJsonAsync<TokenDTO>();
                if (tokenDTO == null)
                {
                    return false;
                }

                User = await LoadUserAndSetToken(tokenDTO);
                if (User != null)
                {
                    SaveTokens(tokenDTO, User.Id.ToString());
                    return true;
                }
            }
            else
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            return false;
        }

        /// <summary>
        /// Set access token to authorization and load user
        /// </summary>
        /// <param name="tokenDTO"></param>
        /// <returns></returns>
        private static async Task<User?> LoadUserAndSetToken(TokenDTO tokenDTO)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDTO.AccessToken);

            try
            {
                var profile = await Client.GetFromJsonAsync<User>("api/User/me");
                if (profile == null)
                {
                    Console.WriteLine("Unable to get user informations.");
                }
                return profile;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Request error: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Save refresh token and user id to credentials and save access token to user
        /// </summary>
        /// <param name="tokenDTO"></param>
        /// <param name="userId"></param>
        private static void SaveTokens(TokenDTO tokenDTO, string userId)
        {
            var cred = new NetworkCredential(userId, tokenDTO.RefreshToken);
            CredentialManager.SaveCredentials("ExpenseTracker-App", cred);

            User.AccessToken = tokenDTO.AccessToken;
        }

        /// <summary>
        /// Remove data from credentials
        /// </summary>
        /// <returns></returns>
        private static async Task Logout()
        {
            Console.Clear();
            CredentialManager.RemoveCredentials("ExpenseTracker-App");
            if(!await Login())
            {
                Console.ReadKey();
                return;
            }
            await Menu();
        }

        /// <summary>
        /// Program Menu
        /// </summary>
        /// <returns></returns>
        private static async Task Menu()
        {
            Console.Clear();

            bool menu = true;
            while (menu)
            {
                Console.WriteLine("=========================");
                Console.WriteLine(User.Email);
                Console.WriteLine("=========================");
                Console.WriteLine("E. End app");
                Console.WriteLine("L. Logout");
                Console.WriteLine("1. Add expense");
                Console.WriteLine("2. View expenses");
                Console.WriteLine("3. View expense");
                Console.WriteLine("4. Edit expense");
                Console.WriteLine("5. Delete expense");
                Console.Write("Your option: ");
                string option = Console.ReadLine();
                switch (option.ToLower())
                {
                    case "e":
                        menu = false;
                        break;
                    case "l":
                        await Logout();
                        menu = false;
                        break;
                    case "1":
                        await AddExpense();
                        break;
                    case "2":
                        await ViewExpenses();
                        break;
                    case "3":
                        await ViewExpenseAsync();
                        break;
                    case "4":
                        await EditExpense();
                        break;
                    case "5":
                        await DeleteExpense();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Asks for expense details and call API to add
        /// </summary>
        /// <returns></returns>
        private static async Task AddExpense()
        {
            Console.Clear();
            Console.Write("Amount: ");
            int amount = int.Parse(Console.ReadLine());
            Console.Write("Category (" + string.Join(", ", Categories) + "): ");
            string category = Console.ReadLine();
            Console.Write("Date: ");
            DateTime date = DateTime.SpecifyKind(DateTime.Parse(Console.ReadLine()), DateTimeKind.Utc);
            Console.Write("Note: ");
            string note = Console.ReadLine();

            Expense expense = new Expense(amount, category, date, note);

            try
            {
                var response = await Client.PostAsJsonAsync("api/Expense", expense);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Expense was added.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            BackToMenu();
        }

        /// <summary>
        /// Call API endpoint to view all users expenses and show them
        /// </summary>
        /// <returns></returns>
        private static async Task ViewExpenses()
        {
            Console.Clear();

            try
            {
                var response = await Client.GetFromJsonAsync<List<Expense>>("api/Expense");

                Console.WriteLine("\tAmount\tCategory\tDate\tNote");
                foreach(var expense in response)
                {
                    Console.WriteLine($"{expense.Id}\t{expense.Amount}$\t{expense.Category}\t{expense.Date.ToString("dd.MM.yyyy")}\t{expense.Note}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            BackToMenu();
        }

        /// <summary>
        /// Call API endpoint to show specific expense
        /// </summary>
        /// <returns></returns>
        private static async Task ViewExpenseAsync()
        {
            Console.Clear();

            var expense = await FindExpense();
            if(expense == null)
            {
                Console.WriteLine("Expense not found.");
                BackToMenu();
                return;
            }

            Console.WriteLine($"{expense.Id}\t{expense.Amount}$\t{expense.Category}\t{expense.Date.ToString("dd.MM.yyyy")}\t{expense.Note}");

            BackToMenu();
        }

        /// <summary>
        /// Call API endpoint to edit details of expense
        /// </summary>
        /// <returns></returns>
        private static async Task EditExpense()
        {
            Console.Clear();

            var expense = await FindExpense();
            if (expense == null)
            {
                Console.WriteLine("Expense not found.");
                BackToMenu();
                return;
            }

            Console.WriteLine("Keep empty if you dont want to change.");
            Console.WriteLine($"{expense.Id}\t{expense.Amount}$\t{expense.Category}\t{expense.Date.ToString("dd.MM.yyyy")}\t{expense.Note}");
            Console.Write("Amount: ");
            string amount = Console.ReadLine();
            Console.Write("Category (" + string.Join(", ", Categories) + "): ");
            string category = Console.ReadLine();
            Console.Write("Date: ");
            string date = Console.ReadLine();
            Console.Write("Note: ");
            string note = Console.ReadLine();

            expense.Update(amount, category, date, note);

            try
            {
                var response = await Client.PutAsJsonAsync($"api/Expense/{expense.Id}", expense);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Expense was updated.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            BackToMenu();
        }

        /// <summary>
        /// Call API endpoint to delete expense
        /// </summary>
        /// <returns></returns>
        private static async Task DeleteExpense()
        {
            Console.Clear();

            var expense = await FindExpense();
            if (expense == null)
            {
                Console.WriteLine("Expense not found.");
                BackToMenu();
                return;
            }

            Console.WriteLine("Do you really want to delete this expense: ");
            Console.WriteLine($"{expense.Id}\t{expense.Amount}$\t{expense.Category}\t{expense.Date.ToString("dd.MM.yyyy")}\t{expense.Note}");
            Console.Write("(Y/N): ");
            string option = Console.ReadLine().ToLower();
            if(option == "y")
            {
                try
                {
                    var response = await Client.DeleteAsync($"api/Expense/{expense.Id}");

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Expense was deleted.");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            BackToMenu();
        }

        /// <summary>
        /// Asks for ID in console and returns expense from API
        /// </summary>
        /// <returns></returns>
        private static async Task<Expense?> FindExpense()
        {
            Console.Write("Expense ID: ");
            int expenseId = int.Parse(Console.ReadLine());

            try
            {
                var response = await Client.GetAsync($"api/Expense/{expenseId}");

                if (response.IsSuccessStatusCode)
                {
                    var expense = await response.Content.ReadFromJsonAsync<Expense>();
                    return expense;
                }

                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Show back to menu message and clear console
        /// </summary>
        private static void BackToMenu()
        {
            Console.WriteLine("Press any key to continue back to menu.");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
