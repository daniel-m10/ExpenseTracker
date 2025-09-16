using ExpenseTracker.Domain.Abstractions;
using ExpenseTracker.Domain.Models;
using ExpenseTracker.Domain.Results;

namespace ExpenseTracker.Domain.Validations
{
    public class ExpenseValidation : IValidation<Expense>
    {
        private const int MaxDescriptionLength = 200;
        private const decimal MaxAmount = 100000m;
        private const int MinYear = 2000;

        public Result Validate(Expense expense)
        {
            if (expense == null)
                return Result.Failure("Expense cannot be null.");

            var errors = new List<string>();

            if (expense.Id == default)
                errors.Add("Id must not be empty.");

            if (string.IsNullOrWhiteSpace(expense.Description))
                errors.Add("Description is required.");
            else if (expense.Description.Trim().Length > MaxDescriptionLength)
                errors.Add("Description length must be less than or equal to 200 characters.");

            if (expense.Money.Amount <= 0)
                errors.Add("Amount must be greater than 0.");

            if (expense.Money.Amount > MaxAmount)
                errors.Add("Amount must be less than or equal to 100,000.");

            if (expense.Date == default)
                errors.Add("Date is required.");
            else if (expense.Date.Year < MinYear)
                errors.Add("Date must not be before the year 2000.");

            if (expense.Date > DateTime.UtcNow)
                errors.Add("Date cannot be in the future.");

            if (expense.CategoryId == default)
                errors.Add("CategoryId must not be empty.");

            if (expense.CreatedAt == default)
                errors.Add("CreatedAt is required.");
            else if (expense.CreatedAt > DateTime.UtcNow)
                errors.Add("CreatedAt cannot be in the future.");

            return errors.Count == 0
                ? Result.Success()
                : Result.Failure(errors);
        }
    }
}
