using ExpenseTracker.Application.Dto;
using ExpenseTracker.Application.Validators;
using FluentValidation.TestHelper;

namespace ExpenseTracker.Application.Tests.Validators
{
    [TestFixture]
    public class CreateExpenseDtoValidatorTests
    {
        private CreateExpenseDtoValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateExpenseDtoValidator();
        }

        [Test]
        public void Should_Pass_When_All_Fields_Are_Valid()
        {
            // Arrange
            var dto = new CreateExpenseDto
            {
                Amount = 50.0m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow.AddMinutes(-1),
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Fail_When_Amount_Is_Zero()
        {
            // Arrange
            var dto = new CreateExpenseDto
            {
                Amount = 0.0m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow.AddMinutes(-1),
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Amount)
                  .WithErrorMessage("Amount must be greater than zero.");
        }

        [Test]
        public void Should_Fail_When_Amount_Is_Negative()
        {
            // Arrange
            var dto = new CreateExpenseDto
            {
                Amount = -10.0m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow.AddMinutes(-1),
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Amount)
                  .WithErrorMessage("Amount must be greater than zero.");
        }

        [Test]
        public void Should_Fail_When_Description_Exceeds_Max_Length()
        {
            // Arrange
            var dto = new CreateExpenseDto
            {
                Amount = 50.0m,
                Description = new string('A', 201), // 201 characters
                Date = DateTime.UtcNow.AddMinutes(-1),
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("Description cannot exceed 200 characters.");
        }

        [Test]
        public void Should_Fail_When_Date_Is_In_The_Future()
        {
            // Arrange
            var dto = new CreateExpenseDto
            {
                Amount = 50.0m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow.AddMinutes(10), // Future date
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Date)
                  .WithErrorMessage("Date cannot be in the future.");
        }

        [Test]
        public void Should_Fail_When_CategoryId_Is_Empty()
        {
            // Arrange
            var dto = new CreateExpenseDto
            {
                Amount = 50.0m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow.AddMinutes(-1),
                CategoryId = Guid.Empty
            };
            // Act
            var result = _validator.TestValidate(dto);
            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                  .WithErrorMessage("CategoryId is required");
        }
    }
}
