using ExpenseTracker.Domain.Abstractions;
using ExpenseTracker.Domain.Models;
using ExpenseTracker.Domain.Results;

namespace ExpenseTracker.Domain.Validations
{
    public class CategoryValidation : IValidation<Category>
    {
        private const int MaxNameLength = 100;
        private const int MaxDescriptionLength = 200;

        public Result Validate(Category category)
        {
            if (category == null)
                return Result.Failure("Category cannot be null.");

            var errors = new List<string>();

            if (category.Id == default)
                errors.Add("Id must not be empty.");

            if (string.IsNullOrWhiteSpace(category.Name))
                errors.Add("Name is required.");
            else if (category.Name.Trim().Length > MaxNameLength)
                errors.Add("Name length must be less than or equal to 100 characters.");

            if (!string.IsNullOrWhiteSpace(category.Description) && category.Description.Trim().Length > MaxDescriptionLength)
                errors.Add("Description length must be less than or equal to 200 characters.");

            if (category.CreatedAt == default)
                errors.Add("CreatedAt is required.");
            else if (category.CreatedAt > DateTime.UtcNow)
                errors.Add("CreatedAt cannot be in the future.");

            return errors.Count == 0
                    ? Result.Success()
                    : Result.Failure(errors);
        }
    }
}
