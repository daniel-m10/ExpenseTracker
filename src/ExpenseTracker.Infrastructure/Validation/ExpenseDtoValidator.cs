using ExpenseTracker.Infrastructure.Repository.Dto;

namespace ExpenseTracker.Infrastructure.Validation
{
    public static class ExpenseDtoValidator
    {
        public static ValidationResult Validate(ExpenseDto dto)
        {
            var errors = new List<string>();

            if (dto.Id == Guid.Empty) errors.Add("Id is required.");
            if (string.IsNullOrWhiteSpace(dto.Description)) errors.Add("Description is required.");
            if (dto.Amount <= 0) errors.Add("Amount must be positive.");
            if (dto.CategoryId == Guid.Empty) errors.Add("CategoryId is required.");

            return new ValidationResult(errors.Count == 0, errors);
        }
    }
}
