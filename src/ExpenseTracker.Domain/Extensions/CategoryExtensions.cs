using ExpenseTracker.Domain.Abstractions;
using ExpenseTracker.Domain.Models;
using ExpenseTracker.Domain.Results;
using ExpenseTracker.Domain.Validations;

namespace ExpenseTracker.Domain.Extensions
{
    public static class CategoryExtensions
    {
        public static Result<Category> UpdateDetails(this Category category, string name, string description)
        {
            if (category == null)
                return Result<Category>.Failure("Category cannot be null.");

            var result = ValidateIfCategoryCanBeUpdated(category, name, description);

            if (result.IsSuccess)
            {
                var updatedCategory = category with { Name = name, Description = description };
                return Result<Category>.Success(updatedCategory);
            }
            return Result<Category>.Failure(result.Errors);
        }

        public static async Task<bool> IsNameUnique(this Category _, string name, ICategoryRepository repository)
        {
            var categories = await repository.GetAllAsync();

            return !categories.Any(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        private static Result ValidateIfCategoryCanBeUpdated(Category category, string name, string description)
        {
            var validation = new CategoryValidation();

            var tempCategory = category with { Name = name, Description = description };

            return validation.Validate(tempCategory);
        }
    }
}
