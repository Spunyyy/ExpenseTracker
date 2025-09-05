using ExpenseTracker.MAUI.Services;
using ExpenseTracker.MAUI.Models.Expense;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Views.Pages;

public partial class MainPage : ContentPage
{
	private readonly ExpensesService service;

	public MainPage(ExpensesService service)
	{
		InitializeComponent();

		this.service = service;
		LoadExpenses();
	}

    /// <summary>
	/// Loads list of expenses and update collection view.
	/// </summary>
	/// <returns></returns>
    private async Task LoadExpenses()
	{
		await service.GetExpensesAsync();
		expensesCollectionView.ItemsSource = service.Expenses;
	}

	/// <summary>
	/// Navigates to EditExpensePage.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    private void ExpenseView_EditExpense(object sender, Expense e)
    {
		Shell.Current.GoToAsync($"{nameof(EditExpensePage)}?Id={e.Id}");
    }

	/// <summary>
	/// Handles deletion of expense after user confirmation.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
    private async void ExpenseView_DeleteExpense(object sender, Expense e)
    {
		if(await DisplayAlert("Confirm", $"Do you really want delete expense from {e.Date.ToLocalTime().ToShortDateString()}, {e.Amount}$", "Yes", "No"))
		{
			if(await service.DeleteExpenseAsync(e.Id))
			{
				await DisplayAlert("Success", "Expense was deleted.", "Ok");
			}
		}
    }
}