namespace ExpenseTracker.API.Models
{
    public class CategoryExpenseDTO
    {
        public string Category { get; set; } = string.Empty;
        public int TotalAmount { get; set; }
    }
}
