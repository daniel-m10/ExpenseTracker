namespace ExpenseTracker.Domain.Exceptions
{
    public class InvalidAmountException(string message) : DomainException(message) { }
}
