using ExpenseTracker.MAUI.Models.Expense;
using ExpenseTracker.MAUI.Services;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Views.Pages;

[QueryProperty(nameof(ExpenseId), "Id")]
public partial class EditExpensePage : ContentPage
{
	private Expense? expense;
	private readonly ExpensesService service;

	public EditExpensePage(ExpensesService service)
	{
		InitializeComponent();
		this.service = service;
        categoryPicker.ItemsSource = new string[] { "Food", "Electronic", "Entertainment", "Transport" };
    }

	public string ExpenseId
	{
		set
		{
			if(int.TryParse(value, out int id))
			{
				_ = LoadExpenseAsync(id);
			}
		}
	}

    /// <summary>
    /// Loads expense by its id and sets it as binding context.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task LoadExpenseAsync(int id)
    {
        expense = await service.GetExpenseAsync(id);
        BindingContext = expense;
    }

    /// <summary>
    /// Navigates to previous page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cancelButton_Clicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// Handles click event for save button, validating input and updating expense.
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
        if (categoryPicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Missing selected category.", "Ok");
            return;
        }

        expense.Date = expense.Date.ToUniversalTime();
		if(await service.UpdateExpenseAsync(expense))
        {
            await DisplayAlert("Success", "Expense updated.", "Ok");
            await Shell.Current.GoToAsync("..");
            return;
        }
        await DisplayAlert("Error", "Something went wrong.", "Ok");
    }
}