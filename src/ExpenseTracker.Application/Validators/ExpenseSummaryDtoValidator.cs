using ExpenseTracker.Application.Dto;
using FluentValidation;

namespace ExpenseTracker.Application.Validators
{
    public class ExpenseSummaryDtoValidator : AbstractValidator<ExpenseSummaryDto>
    {
        public ExpenseSummaryDtoValidator()
        {
            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("TotalAmount must be greater than or equal to zero.");
            RuleFor(x => x.ExpenseCount)
                .GreaterThanOrEqualTo(0).WithMessage("ExpenseCount must be greater than or equal to zero.");
            RuleFor(x => x.CategorySummaries)
                .NotNull().WithMessage("CategorySummaries is required.")
                .NotEmpty().WithMessage("Category summaries should not be empty.");
            RuleForEach(x => x.CategorySummaries)
                .NotNull().WithMessage("CategorySummaryDto is required.")
                .SetValidator(new CategorySummaryDtoValidator());
            RuleForEach(x => x.CategorySummaries)
                .SetValidator(new CategorySummaryDtoValidator());
        }
    }
}
