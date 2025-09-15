using ExpenseTracker.Domain.Results;

namespace ExpenseTracker.Domain.Abstractions
{
    public interface IValidation<T>
    {
        Result Validate(T validationObject);
    }
}
