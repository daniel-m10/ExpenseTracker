using ExpenseTracker.Domain.ValueObjects;

namespace ExpenseTracker.Domain.Models
{
    public record Expense
    {
        public Guid Id { get; init; }
        public string Description { get; init; } = string.Empty;
        public Money Money { get; init; } = new Money(amount: 0);
        public DateTime Date { get; init; }
        public Guid CategoryId { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
