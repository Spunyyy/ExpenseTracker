using ExpenseTracker.MAUI.Views.Pages;

namespace ExpenseTracker.MAUI;

public partial class AuthShell : Shell
{
	public AuthShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
    }
}