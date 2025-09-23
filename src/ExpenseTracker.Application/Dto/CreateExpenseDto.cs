namespace ExpenseTracker.Application.Dto
{
    public record CreateExpenseDto
    {
        public required decimal Amount { get; init; }
        public string? Description { get; init; }
        public required DateTime Date { get; init; }
        public required Guid CategoryId { get; init; }
    }
}
