using ExpenseTracker.MAUI.Services;
using ExpenseTracker.MAUI.Views.Pages;

namespace ExpenseTracker.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(AddExpensePage), typeof(AddExpensePage));
            Routing.RegisterRoute(nameof(EditExpensePage), typeof(EditExpensePage));
        }

        private void logoutButton_Clicked(object sender, EventArgs e)
        {
            TokenStorage.ClearAll();

            Application.Current.MainPage = new AuthShell();
        }
    }
}
