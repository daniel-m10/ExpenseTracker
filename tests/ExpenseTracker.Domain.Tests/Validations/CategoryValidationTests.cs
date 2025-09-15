using ExpenseTracker.Domain.Models;
using ExpenseTracker.Domain.Validations;

namespace ExpenseTracker.Domain.Tests.Validations
{
    [TestFixture]
    public class CategoryValidationTests
    {
        private CategoryValidation _validation;

        [SetUp]
        public void SetUp()
        {
            _validation = new CategoryValidation();
        }

        [Test]
        public void CategoryValidation_Should_Return_Success_For_Valid_Category()
        {
            // Arrange
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Description = "Test Description",
                CreatedAt = DateTime.Now
            };

            // Act
            var result = _validation.Validate(category);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Errors, Is.Empty);
            }
        }

        [Test]
        public void CategoryValidation_Should_Return_Error_When_Category_Is_Null()
        {
            // Act
            var result = _validation.Validate(null!);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Category cannot be null."));
            }
        }

        [Test]
        public void CategoryValidation_Should_Return_Error_When_Id_Is_Default()
        {
            // Arrange
            var category = new Category
            {
                Id = default,
                Name = "Test",
                Description = "Test Description",
                CreatedAt = DateTime.Now
            };

            // Act
            var result = _validation.Validate(category);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Id must not be empty."));
            }
        }

        [Test]
        public void CategoryValidation_Should_Return_Error_When_Name_Is_Empty()
        {
            // Arrange
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Description = "Test Description",
                CreatedAt = DateTime.Now
            };

            // Act
            var result = _validation.Validate(category);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Name is required."));
            }
        }

        [Test]
        public void CategoryValidation_Should_Return_Error_When_Name_Exceeds_Max_Length()
        {
            // Arrange
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = new string('A', 101),
                Description = "Test Description",
                CreatedAt = DateTime.Now
            };

            // Act
            var result = _validation.Validate(category);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Name length must be less than or equal to 100 characters."));
            }
        }

        [Test]
        public void CategoryValidation_Should_Return_Error_When_Description_Exceeds_Max_Length()
        {
            // Arrange
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Description = new string('A', 201),
                CreatedAt = DateTime.Now
            };

            // Act
            var result = _validation.Validate(category);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Description length must be less than or equal to 200 characters."));
            }
        }

        [Test]
        public void CategoryValidation_Should_Return_Error_When_CreatedAt_Is_Default()
        {
            // Arrange
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Description = "Test Description",
                CreatedAt = default
            };

            // Act
            var result = _validation.Validate(category);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("CreatedAt is required."));
            }
        }

        [Test]
        public void CategoryValidation_Should_Return_Error_When_CreatedAt_Is_In_The_Future()
        {
            // Arrange
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Description = "Test Description",
                CreatedAt = DateTime.UtcNow.AddMinutes(1)
            };

            // Act
            var result = _validation.Validate(category);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("CreatedAt cannot be in the future."));
            }
        }

        [Test]
        public void CategoryValidation_Should_Return_Multiple_Errors_For_Multiple_Invalid_Fields()
        {
            // Arrange
            var category = new Category
            {
                Id = default,
                Name = string.Empty,
                Description = string.Empty,
                CreatedAt = default
            };

            // Act
            var result = _validation.Validate(category);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(3));
                Assert.That(result.Errors, Does.Contain("Id must not be empty."));
                Assert.That(result.Errors, Does.Contain("Name is required."));
                Assert.That(result.Errors, Does.Contain("CreatedAt is required."));
            }
        }
    }
}
