namespace ExpenseTracker.Application.Dto
{
    public record ExpenseSummaryDto
    {
        public required decimal TotalAmount { get; init; }
        public required int ExpenseCount { get; init; }
        public required IEnumerable<CategorySummaryDto> CategorySummaries { get; init; }
    }
}
