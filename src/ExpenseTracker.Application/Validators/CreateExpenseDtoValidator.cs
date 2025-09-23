using ExpenseTracker.Application.Dto;
using FluentValidation;

namespace ExpenseTracker.Application.Validators
{
    public class CreateExpenseDtoValidator : AbstractValidator<CreateExpenseDto>
    {
        public CreateExpenseDtoValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
            RuleFor(x => x.Description)
                .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");
            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required");
        }
    }
}
