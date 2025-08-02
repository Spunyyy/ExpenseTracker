using ExpenseTracker.API.Models;

namespace ExpenseTracker.API.Entities
{
    public class Expense
    {
        public int Id { get; private set; }
        public int Amount { get; private set; }
        public string Category { get; private set; } = string.Empty;
        public DateTime Date { get; private set; }
        public string? Note { get; private set; }
        public Guid UserId { get; private set; }

        public Expense()
        {
        }

        public Expense(ExpenseDTO dto, Guid userId)
        {
            Amount = dto.Amount;
            Category = dto.Category; 
            Date = (dto.Date ?? DateTime.UtcNow);
            Note = dto.Note;
            UserId = userId;
        }

        public void CopyFromDTO(ExpenseDTO dto)
        {
            Amount = dto.Amount;
            Category = dto.Category;
            Date = (dto.Date ?? DateTime.UtcNow);
            Note = dto.Note;
        }

    }
}
