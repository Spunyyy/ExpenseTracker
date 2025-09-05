using ExpenseTracker.MAUI.Models.Expense;

namespace ExpenseTracker.MAUI.Views.Controls;

public partial class ExpenseView : ContentView
{
    /// <summary>
    /// Raised when edit button is clicked.
    /// </summary>
    public event EventHandler<Expense> EditExpense;
    /// <summary>
    /// Raised when delete button is clicked.
    /// </summary>
    public event EventHandler<Expense> DeleteExpense;

	public ExpenseView()
	{
		InitializeComponent();
	}

    private void editButton_Clicked(object sender, EventArgs e)
    {
        if(BindingContext is Expense expense)
        {
            EditExpense?.Invoke(this, expense);
        }
    }

    private void deleteButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Expense expense)
        {
            DeleteExpense?.Invoke(this, expense);
        }
    }
}