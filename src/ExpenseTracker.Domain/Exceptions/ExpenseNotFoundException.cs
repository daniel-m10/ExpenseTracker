namespace ExpenseTracker.Domain.Exceptions
{
    public class ExpenseNotFoundException(string message) : DomainException(message) { }
}
