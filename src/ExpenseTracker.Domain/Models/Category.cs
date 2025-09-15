namespace ExpenseTracker.Domain.Models
{
    public record Category
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
