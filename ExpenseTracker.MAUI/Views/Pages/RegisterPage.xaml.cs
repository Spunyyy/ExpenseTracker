using ExpenseTracker.MAUI.Models.Auth;
using ExpenseTracker.MAUI.Services;

namespace ExpenseTracker.MAUI.Views.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly AuthService auth;

    public RegisterPage(AuthService auth)
	{
		InitializeComponent();

        this.auth = auth;
	}

    /// <summary>
    /// Navigates to LoginPage.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void backToLoginButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//" + nameof(LoginPage));
    }

    /// <summary>
    /// Handles click event for register button, validating user input and attempting to register a new account.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void registerButton_Clicked(object sender, EventArgs e)
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
        if (passwordRepeatValidator.IsNotValid)
        {
            await DisplayAlert("Error", "Missing repeated password.", "Ok");
            return;
        }

        if(passwordEntry.Text != passwordRepeatEntry.Text)
        {
            await DisplayAlert("Error", "Password doesn't match.", "Ok");
            return;
        }

        var request = new LoginRequest
        {
            Email = emailEntry.Text,
            Password = passwordEntry.Text
        };

        if(await auth.RegisterAsync(request))
        {
            await DisplayAlert("Success", "Successfully registered. Now you can login.", "Ok");
            await Shell.Current.GoToAsync("//" + nameof(LoginPage));
        }
        else
        {
            await DisplayAlert("Error", "User already exists.", "Ok");
        }
    }
}