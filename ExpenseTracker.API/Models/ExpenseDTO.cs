namespace ExpenseTracker.API.Models
{
    public class ExpenseDTO
    {
        public int Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public string? Note { get; set; }
    }
}
