namespace ExpenseTracker.Application.Dto
{
    public record CategorySummaryDto
    {
        public required Guid CategoryId { get; init; }
        public required string CategoryName { get; init; }
        public required double TotalAmount { get; init; }
        public required int ExpenseCount { get; init; }
    }
}
