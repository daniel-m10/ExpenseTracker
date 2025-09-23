namespace ExpenseTracker.Application.Dto
{
    public record ExpenseDto
    {
        public required Guid Id { get; init; }
        public required decimal Amount { get; init; }
        public string? Description { get; init; }
        public required DateTime Date { get; init; }
        public required CategoryDto Category { get; init; }
    }
}
