namespace ExpenseTracker.Domain.Models
{
    public record Expense
    {
        public Guid Id { get; init; }
        public string Description { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public DateTime Date { get; init; }
        public Guid CategoryId { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
