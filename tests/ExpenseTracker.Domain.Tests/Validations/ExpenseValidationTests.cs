using ExpenseTracker.Domain.Models;
using ExpenseTracker.Domain.Validations;
using ExpenseTracker.Domain.ValueObjects;

namespace ExpenseTracker.Domain.Tests.Validations
{
    [TestFixture]
    public class ExpenseValidationTests
    {
        private ExpenseValidation _validation;

        [SetUp]
        public void SetUp()
        {
            _validation = new ExpenseValidation();
        }

        [Test]
        public void ExpenseValidation_Should_Return_Success_For_Valid_Expense()
        {
            // Arrange
            var money = new Money(amount: 2);
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Money = money,
                Date = DateTime.UtcNow,
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Errors, Is.Empty);
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_Expense_Is_Null()
        {
            // Act
            var result = _validation.Validate(null!);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Expense cannot be null."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_Id_Is_Default()
        {
            // Arrange
            var money = new Money(amount: 2);
            var expense = new Expense
            {
                Id = default,
                Description = "Description",
                Money = money,
                Date = DateTime.UtcNow,
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Id must not be empty."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_Description_Is_Empty()
        {
            // Arrange
            var money = new Money(amount: 2);
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = string.Empty,
                Money = money,
                Date = DateTime.UtcNow,
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Description is required."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_Description_Exceeds_Max_Length()
        {
            // Arrange
            var money = new Money(amount: 2);
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = new string('A', 201),
                Money = money,
                Date = DateTime.UtcNow,
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Description length must be less than or equal to 200 characters."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_Amount_Is_Zero()
        {
            // Arrange
            var money = new Money(amount: 0);
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Money = money,
                Date = DateTime.UtcNow,
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Amount must be greater than 0."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_Amount_Exceeds_Max()
        {
            // Arrange
            var money = new Money(amount: 100001);
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Money = money,
                Date = DateTime.UtcNow,
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Amount must be less than or equal to 100,000."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_Date_Is_Default()
        {
            // Arrange
            var money = new Money(amount: 2);
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Money = money,
                Date = default,
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Date is required."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_Date_Is_In_The_Future()
        {
            // Arrange
            var money = new Money(amount: 2);
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Money = money,
                Date = new DateTime(2026, 1, 1),
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Date cannot be in the future."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_Date_Is_Before_MinYear()
        {
            // Arrange
            var money = new Money(amount: 2);
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Money = money,
                Date = new DateTime(1999, 1, 1),
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Date must not be before the year 2000."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_CategoryId_Is_Default()
        {
            // Arrange
            var money = new Money(amount: 2);
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Money = money,
                Date = DateTime.UtcNow,
                CategoryId = default,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("CategoryId must not be empty."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_CreatedAt_Is_Default()
        {
            // Arrange
            var money = new Money(amount: 2);
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Money = money,
                Date = DateTime.UtcNow,
                CategoryId = Guid.NewGuid(),
                CreatedAt = default
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("CreatedAt is required."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Error_When_CreatedAt_Is_In_The_Future()
        {
            // Arrange
            var money = new Money(amount: 2);
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Money = money,
                Date = DateTime.UtcNow,
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow.AddMinutes(1)
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("CreatedAt cannot be in the future."));
            }
        }

        [Test]
        public void ExpenseValidation_Should_Return_Multiple_Errors_For_Multiple_Invalid_Fields()
        {
            // Arrange
            var money = new Money(amount: 0);
            var expense = new Expense
            {
                Id = default,
                Description = string.Empty,
                Money = money,
                Date = default,
                CategoryId = default,
                CreatedAt = default
            };

            // Act
            var result = _validation.Validate(expense);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(6));
                Assert.That(result.Errors, Does.Contain("Id must not be empty."));
                Assert.That(result.Errors, Does.Contain("Description is required."));
                Assert.That(result.Errors, Does.Contain("Amount must be greater than 0."));
                Assert.That(result.Errors, Does.Contain("Date is required."));
                Assert.That(result.Errors, Does.Contain("CategoryId must not be empty."));
                Assert.That(result.Errors, Does.Contain("CreatedAt is required."));
            }
        }
    }
}
