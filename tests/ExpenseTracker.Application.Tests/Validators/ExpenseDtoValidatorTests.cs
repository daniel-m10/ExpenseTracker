using ExpenseTracker.Application.Dto;
using ExpenseTracker.Application.Validators;
using FluentValidation.TestHelper;

namespace ExpenseTracker.Application.Tests.Validators
{
    [TestFixture]
    public class ExpenseDtoValidatorTests
    {
        private ExpenseDtoValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new ExpenseDtoValidator();
        }

        [Test]
        public void Should_Pass_When_All_Fields_Are_Valid()
        {
            // Arrange
            var dto = new ExpenseDto
            {
                Id = Guid.NewGuid(),
                Amount = 50.0m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow.AddMinutes(-1),
                Category = new CategoryDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Food"
                }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_Fail_When_Id_Is_Empty()
        {
            // Arrange
            var dto = new ExpenseDto
            {
                Id = Guid.Empty,
                Amount = 50.0m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow.AddMinutes(-1),
                Category = new CategoryDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Food"
                }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage("Id is required");
        }

        [Test]
        public void Should_Fail_When_Amount_Is_Negative()
        {
            // Arrange
            var dto = new ExpenseDto
            {
                Id = Guid.NewGuid(),
                Amount = -10.0m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow.AddMinutes(-1),
                Category = new CategoryDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Food"
                }
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
            var dto = new ExpenseDto
            {
                Id = Guid.NewGuid(),
                Amount = 50.0m,
                Description = new string('a', 201), // Exceeds max length of 200
                Date = DateTime.UtcNow.AddMinutes(-1),
                Category = new CategoryDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Food"
                }
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
            var dto = new ExpenseDto
            {
                Id = Guid.NewGuid(),
                Amount = 50.0m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow.AddMinutes(10), // Future date
                Category = new CategoryDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Food"
                }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Date)
                  .WithErrorMessage("Date cannot be in the future.");
        }

        [Test]
        public void Should_Fail_When_Category_Is_Null()
        {
            // Arrange
            var dto = new ExpenseDto
            {
                Id = Guid.NewGuid(),
                Amount = 50.0m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow.AddMinutes(-1),
                Category = null!
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Category)
                  .WithErrorMessage("Category is required.");
        }

        [Test]
        public void Should_Fail_When_Category_Is_Invalid()
        {
            // Arrange
            var dto = new ExpenseDto
            {
                Id = Guid.NewGuid(),
                Amount = 50.0m,
                Description = "Grocery shopping",
                Date = DateTime.UtcNow.AddMinutes(-1),
                Category = new CategoryDto
                {
                    Id = Guid.Empty, // Invalid Id
                    Name = ""       // Invalid Name
                }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor("Category.Id")
                  .WithErrorMessage("Id is required");
            result.ShouldHaveValidationErrorFor("Category.Name")
                  .WithErrorMessage("Name is required");
        }
    }
}
