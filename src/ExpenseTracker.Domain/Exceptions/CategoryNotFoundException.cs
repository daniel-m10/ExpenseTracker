namespace ExpenseTracker.Domain.Exceptions
{
    public class CategoryNotFoundException(string message) : DomainException(message) { }
}
