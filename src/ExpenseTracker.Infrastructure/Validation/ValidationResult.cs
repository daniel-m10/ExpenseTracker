namespace ExpenseTracker.Infrastructure.Validation
{
    public record ValidationResult(bool IsValid, IEnumerable<string> Errors);
}
