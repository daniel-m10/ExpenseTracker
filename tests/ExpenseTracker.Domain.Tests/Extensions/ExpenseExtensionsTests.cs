using ExpenseTracker.Domain.Extensions;
using ExpenseTracker.Domain.Models;

namespace ExpenseTracker.Domain.Tests.Extensions
{
    [TestFixture]
    public class ExpenseExtensionsTests
    {
        [Test]
        public void ExpenseExtensions_UpdateDetails_Should_Return_Success_For_Valid_Update()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expense = new Expense { Id = id, Description = "A", Amount = 1, Date = DateTime.Today, CategoryId = id, CreatedAt = DateTime.Today };

            // Act
            var updatedId = Guid.NewGuid();
            var result = expense.UpdateDetails(description: "B", amount: 2, date: DateTime.Today.AddMinutes(5), categoryId: updatedId);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Errors, Is.Empty);
                Assert.That(result.Value?.Id, Is.EqualTo(id));
                Assert.That(result.Value?.Description, Is.EqualTo("B"));
                Assert.That(result.Value?.Amount, Is.EqualTo(2));
                Assert.That(result.Value?.CategoryId, Is.EqualTo(updatedId));
            }
        }

        [Test]
        public void ExpenseExtensions_UpdateDetails_Should_Return_Failure_For_Invalid_Update()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expense = new Expense { Id = id, Description = "A", Amount = 1, Date = DateTime.Today, CategoryId = id, CreatedAt = DateTime.Today };

            // Act
            var updatedId = Guid.NewGuid();
            var result = expense.UpdateDetails(description: string.Empty, amount: 2, date: DateTime.Today.AddMinutes(5), categoryId: updatedId);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Errors, Does.Contain("Description is required."));
            }
        }

        [Test]
        public void ExpenseExtensions_UpdateDetails_Should_Return_Failure_When_Expense_Is_Null()
        {
            // Arrange
            Expense expense = null!;

            // Act
            var updatedId = Guid.NewGuid();
            var result = expense.UpdateDetails(description: string.Empty, amount: 2, date: DateTime.Today.AddMinutes(5), categoryId: updatedId);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Errors, Does.Contain("Expense cannot be null."));
            }
        }

        [Test]
        public void ExpenseExtensions_UpdateDetails_Should_Not_Mutate_Original_Expense()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expense = new Expense { Id = id, Description = "A", Amount = 1, Date = DateTime.Today, CategoryId = id, CreatedAt = DateTime.Today };

            // Act
            var updatedId = Guid.NewGuid();
            var result = expense.UpdateDetails(description: "B", amount: 2, date: DateTime.Today.AddMinutes(5), categoryId: updatedId);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Errors, Is.Empty);
                Assert.That(expense, Is.Not.EqualTo(result.Value));
            }
        }

        [Test]
        public void ExpenseExtensions_IsInMonth_Should_Return_True_When_Date_Matches()
        {
            // Arrange
            var date = DateTime.Today;
            var year = date.Year;
            var month = date.Month;

            var expense = new Expense { Date = date };

            // Act
            var result = expense.IsInMonth(year, month);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ExpenseExtensions_IsInMonth_Should_Return_False_When_Date_Does_Not_Match()
        {
            // Arrange
            var date = DateTime.Today;
            var year = 2024;
            var month = date.Month;

            var expense = new Expense { Date = date };

            // Act
            var result = expense.IsInMonth(year, month);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void ExpenseExtensions_IsInDateRange_Should_Return_True_When_Date_In_Range()
        {
            // Arrange
            var start = DateTime.Today;
            var end = start.AddDays(10);

            var expense = new Expense { Date = DateTime.Today };

            // Act
            var result = expense.IsInDateRange(start, end);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ExpenseExtensions_IsInDateRange_Should_Return_False_When_Date_Out_Of_Range()
        {
            // Arrange
            var start = DateTime.Today.AddDays(-5);
            var end = start.AddDays(4);

            var expense = new Expense { Date = DateTime.Today };

            // Act
            var result = expense.IsInDateRange(start, end);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
