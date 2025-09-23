using ExpenseTracker.Application.Dto;
using ExpenseTracker.Application.Validators;
using FluentValidation.TestHelper;

namespace ExpenseTracker.Application.Tests.Validators
{
    [TestFixture]
    public class CategorySummaryDtoValidatorTests
    {
        private CategorySummaryDtoValidator _validator;
        [SetUp]
        public void Setup()
        {
            _validator = new CategorySummaryDtoValidator();
        }

        [Test]
        public void Should_Pass_When_All_Fields_Are_Valid()
        {
            // Arrange
            var dto = new CategorySummaryDto
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = "Food",
                TotalAmount = 80.0,
                ExpenseCount = 4
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Fail_When_CategoryId_Is_Empty()
        {
            // Arrange
            var dto = new CategorySummaryDto
            {
                CategoryId = Guid.Empty,
                CategoryName = "Food",
                TotalAmount = 80.0,
                ExpenseCount = 4
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                .WithErrorMessage("CategoryId is required");
        }

        [Test]
        public void Should_Fail_When_CategoryName_Is_Empty()
        {
            // Arrange
            var dto = new CategorySummaryDto
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = string.Empty,
                TotalAmount = 80.0,
                ExpenseCount = 4
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategoryName)
                .WithErrorMessage("CategoryName is required");
        }

        [Test]
        public void Should_Fail_When_CategoryName_Exceeds_Max_Length()
        {
            // Arrange
            var dto = new CategorySummaryDto
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = new string('A', 101), // 101 characters
                TotalAmount = 80.0,
                ExpenseCount = 4
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategoryName)
                .WithErrorMessage("CategoryName cannot exceed 100 characters.");
        }

        [Test]
        public void Should_Fail_When_TotalAmount_Is_Negative()
        {
            // Arrange
            var dto = new CategorySummaryDto
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = "Food",
                TotalAmount = -10.0,
                ExpenseCount = 4
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
            var dto = new CategorySummaryDto
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = "Food",
                TotalAmount = 80.0,
                ExpenseCount = -1
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ExpenseCount)
                .WithErrorMessage("ExpenseCount must be greater than or equal to zero.");
        }
    }
}
