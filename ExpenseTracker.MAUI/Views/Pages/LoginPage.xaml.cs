using ExpenseTracker.MAUI.Models.Auth;
using ExpenseTracker.MAUI.Services;

namespace ExpenseTracker.MAUI.Views.Pages;

public partial class LoginPage : ContentPage
{
    private readonly AuthService auth;

    public LoginPage(AuthService auth)
	{
		InitializeComponent();
        this.auth = auth;

    }

    /// <summary>
    /// Navigates to RegisterPage
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void registerButton_Clicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//" + nameof(RegisterPage));
    }

    /// <summary>
    /// Handles login button click event, validating user input and attempting to log in.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void loginButton_Clicked(object sender, EventArgs e)
    {
        if (emailValidator.IsNotValid)
        {
            await DisplayAlert("Error", "Missing email.", "Ok");
            return;
        }
        if (passwordValidator.IsNotValid)
        {
            await DisplayAlert("Error", "Missing password.", "Ok");
            return;
        }

        var request = new LoginRequest
        {
            Email = emailEntry.Text,
            Password = passwordEntry.Text
        };

        if (await auth.LoginAsync(request))
        {
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Error", "Invalid email or password!", "Ok");
        }
    }
}