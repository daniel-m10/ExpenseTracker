using ExpenseTracker.Application.Dto;
using FluentValidation;

namespace ExpenseTracker.Application.Validators
{
    public class CategorySummaryDtoValidator : AbstractValidator<CategorySummaryDto>
    {
        public CategorySummaryDtoValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required");
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("CategoryName is required")
                .MaximumLength(100).WithMessage("CategoryName cannot exceed 100 characters.");
            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("TotalAmount must be greater than or equal to zero.");
            RuleFor(x => x.ExpenseCount)
                .GreaterThanOrEqualTo(0).WithMessage("ExpenseCount must be greater than or equal to zero.");
        }
    }
}
