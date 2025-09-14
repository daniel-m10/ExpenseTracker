namespace ExpenseTracker.Domain.Models
{
    public class Expense
    {
        public Guid Id { get; init; }
        public string Description { get; init; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedAt { get; init; }
    }
}
