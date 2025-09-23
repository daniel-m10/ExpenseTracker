using ExpenseTracker.Application.Dto;
using ExpenseTracker.Application.Validators;
using FluentValidation.TestHelper;

namespace ExpenseTracker.Application.Tests.Validators
{
    [TestFixture]
    public class ExpenseSummaryDtoValidatorTests
    {
        private ExpenseSummaryDtoValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new ExpenseSummaryDtoValidator();
        }

        [Test]
        public void Should_Pass_When_All_Fields_Are_Valid()
        {
            // Arrange
            var dto = new ExpenseSummaryDto
            {
                TotalAmount = 100.0m,
                ExpenseCount = 5,
                CategorySummaries = [
                    new CategorySummaryDto
                    {
                        CategoryId = Guid.NewGuid(),
                        CategoryName = "Food",
                        TotalAmount = 80.0,
                        ExpenseCount = 4
                    }
                ]
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Fail_When_TotalAmount_Is_Negative()
        {
            // Arrange
            var dto = new ExpenseSummaryDto
            {
                TotalAmount = -10.0m,
                ExpenseCount = 5,
                CategorySummaries = [
                    new CategorySummaryDto
                    {
                        CategoryId = Guid.NewGuid(),
                        CategoryName = "Food",
                        TotalAmount = 80.0,
                        ExpenseCount = 4
                    }
                ]
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TotalAmount)
                  .WithErrorMessage("TotalAmount must be greater than or equal to zero.");
        }

        [Test]
        public void Should_Fail_When_ExpenseCount_Is_Negative()
        {
            // Arrange
            var dto = new ExpenseSummaryDto
            {
                TotalAmount = 10.0m,
                ExpenseCount = -1,
                CategorySummaries = [
                    new CategorySummaryDto
                    {
                        CategoryId = Guid.NewGuid(),
                        CategoryName = "Food",
                        TotalAmount = 80.0,
                        ExpenseCount = 4
                    }
                ]
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ExpenseCount)
                .WithErrorMessage("ExpenseCount must be greater than or equal to zero.");
        }

        [Test]
        public void Should_Fail_When_CategorySummaries_Is_Null()
        {
            // Arrange
            var dto = new ExpenseSummaryDto
            {
                TotalAmount = 10.0m,
                ExpenseCount = 5,
                CategorySummaries = null!
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategorySummaries)
                .WithErrorMessage("CategorySummaries is required.");
        }

        [Test]
        public void Should_Fail_When_CategorySummaries_Has_Invalid_Item()
        {
            // Arrange
            var dto = new ExpenseSummaryDto
            {
                TotalAmount = 10.0m,
                ExpenseCount = 5,
                CategorySummaries = [
                    new CategorySummaryDto
                    {
                        CategoryId = Guid.NewGuid(),
                        CategoryName = "Food",
                        TotalAmount = 80.0,
                        ExpenseCount = -10
                    }
                ]
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor("CategorySummaries[0].ExpenseCount")
                .WithErrorMessage("ExpenseCount must be greater than or equal to zero.");
        }

        [Test]
        public void Should_Pass_When_All_Fields_Are_Valid_And_CategorySummaries_Has_Items()
        {
            // Arrange
            var dto = new ExpenseSummaryDto
            {
                TotalAmount = 150.0m,
                ExpenseCount = 10,
                CategorySummaries = [
                    new CategorySummaryDto
                    {
                        CategoryId = Guid.NewGuid(),
                        CategoryName = "Transport",
                        TotalAmount = 70.0,
                        ExpenseCount = 3
                    },
                    new CategorySummaryDto
                    {
                        CategoryId = Guid.NewGuid(),
                        CategoryName = "Entertainment",
                        TotalAmount = 80.0,
                        ExpenseCount = 7
                    }
                ]
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Fail_When_CategorySummaries_Is_Empty()
        {
            // Arrange
            var dto = new ExpenseSummaryDto
            {
                TotalAmount = 10.0m,
                ExpenseCount = 5,
                CategorySummaries = []
            };
            // Act
            var result = _validator.TestValidate(dto);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategorySummaries)
                .WithErrorMessage("Category summaries should not be empty.");
        }

        [Test]
        public void Should_Pass_When_CategorySummaries_Has_One_Item()
        {
            // Arrange
            var dto = new ExpenseSummaryDto
            {
                TotalAmount = 50.0m,
                ExpenseCount = 2,
                CategorySummaries = [
                    new CategorySummaryDto
                    {
                        CategoryId = Guid.NewGuid(),
                        CategoryName = "Utilities",
                        TotalAmount = 50.0,
                        ExpenseCount = 2
                    }
                ]
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Fail_When_CategorySummaries_Contains_Null_Item()
        {
            // Arrange
            var dto = new ExpenseSummaryDto
            {
                TotalAmount = 100.0m,
                ExpenseCount = 5,
                CategorySummaries = [null!]
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor("CategorySummaries[0]")
                .WithErrorMessage("CategorySummaryDto is required.");
        }
    }
}
