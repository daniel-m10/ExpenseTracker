using ExpenseTracker.Domain.Models;
using ExpenseTracker.Domain.Results;
using ExpenseTracker.Domain.Validations;

namespace ExpenseTracker.Domain.Extensions
{
    public static class ExpenseExtensions
    {
        public static Result<Expense> UpdateDetails(this Expense expense, string description, decimal amount, DateTime date, Guid categoryId)
        {
            if (expense is null)
                return Result<Expense>.Failure("Expense cannot be null.");

            var result = ValidateIfExpenseCanBeUpdated(expense, description, amount, date, categoryId);

            if (result.IsSuccess)
            {
                var updatedExpense = expense with
                {
                    Description = description,
                    Amount = amount,
                    Date = date,
                    CategoryId = categoryId,
                };
                return Result<Expense>.Success(updatedExpense);
            }
            return Result<Expense>.Failure(result.Errors);
        }

        public static bool IsInMonth(this Expense expense, int year, int month)
        {
            return expense.Date.Year == year && expense.Date.Month == month;
        }

        public static bool IsInDateRange(this Expense expense, DateTime start, DateTime end)
        {
            return expense.Date >= start && expense.Date <= end;
        }

        private static Result ValidateIfExpenseCanBeUpdated(Expense expense, string description, decimal amount, DateTime date, Guid categoryId)
        {
            var validation = new ExpenseValidation();

            var tempExpense = expense with
            {
                Description = description,
                Amount = amount,
                Date = date,
                CategoryId = categoryId,
            };

            return validation.Validate(tempExpense);
        }
    }
}
