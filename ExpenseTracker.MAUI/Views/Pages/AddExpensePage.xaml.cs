using ExpenseTracker.MAUI.Models.Expense;
using ExpenseTracker.MAUI.Services;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Views.Pages;

public partial class AddExpensePage : ContentPage
{
	private readonly ExpensesService service;

	public AddExpensePage(ExpensesService service)
	{
		InitializeComponent();

		this.service = service;
		categoryPicker.ItemsSource = new string[] { "Food", "Electronic", "Entertainment", "Transport" };
	}

	/// <summary>
	/// Handles click event for save button, validating input fields and creating new expense entry.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    private async void saveButton_Clicked(object sender, EventArgs e)
    {
		if (amountValidator.IsNotValid)
		{
			await DisplayAlert("Error", "Missing amount.", "Ok");
			return;
		}
		if(categoryPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Missing selected category.", "Ok");
            return;
        }

		var dto = new CreateExpense
		{
			Amount = int.Parse(amountEntry.Text),
			Date = datePicker.Date.ToUniversalTime(),
			Category = categoryPicker.SelectedItem.ToString(),
			Note = noteEditor.Text
        };

		if(await service.AddExpenseAsync(dto))
		{
			await DisplayAlert("Success", "Expense added.", "Ok");
			await Shell.Current.GoToAsync("//" + nameof(MainPage));
		}
		else
		{
			await DisplayAlert("Error", "Something went wrong.", "Ok");
		}
    }
}