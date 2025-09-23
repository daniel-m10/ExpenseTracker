using ExpenseTracker.Application.Dto;
using ExpenseTracker.Application.Validators;
using FluentValidation.TestHelper;

namespace ExpenseTracker.Application.Tests.Validators
{
    [TestFixture]
    public class CategoryDtoValidatorTests
    {
        private CategoryDtoValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CategoryDtoValidator();
        }

        [Test]
        public void Should_Pass_When_All_Fields_Are_Valid()
        {
            // Arrange
            var dto = new CategoryDto
            {
                Id = Guid.NewGuid(),
                Name = "Utilities"
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
            var dto = new CategoryDto
            {
                Id = Guid.Empty,
                Name = "Utilities"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id is required");
        }

        [Test]
        public void Should_Fail_When_Name_Is_Empty()
        {
            // Arrange
            var dto = new CategoryDto
            {
                Id = Guid.NewGuid(),
                Name = string.Empty
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name is required");
        }

        [Test]
        public void Should_Fail_When_Name_Exceeds_MaxLength()
        {
            // Arrange
            var dto = new CategoryDto
            {
                Id = Guid.NewGuid(),
                Name = new string('A', 101) // Assuming max length is 100
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name cannot exceed 100 characters.");
        }
    }
}
