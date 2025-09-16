using ExpenseTracker.Domain.Models;
using ExpenseTracker.Domain.ValueObjects;

namespace ExpenseTracker.Domain.Tests.Models
{
    [TestFixture]
    public class ExpenseTests
    {
        [Test]
        public void Expense_Should_SetAndGet_Id()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expense = new Expense()
            {
                Id = id,
            };

            // Assert
            Assert.That(expense.Id, Is.Not.Default);
            Assert.That(expense.Id, Is.EqualTo(id));
        }

        [Test]
        public void Expense_Should_SetAndGet_Description()
        {
            // Arrange
            var description = "Test Description";
            var expense = new Expense
            {
                Description = description
            };

            Assert.That(expense.Description, Is.Not.Null);
            Assert.That(expense.Description, Is.EqualTo(description));
        }

        [Test]
        public void Expense_Should_SetAndGet_Amount()
        {
            // Arrange
            var money = new Money(amount: 2);
            var expense = new Expense
            {
                Money = money
            };

            // Assert
            Assert.That(expense.Money, Is.Not.Default);
            Assert.That(expense.Money.Amount, Is.EqualTo(2));
            Assert.That(expense.Money.Currency, Is.EqualTo("USD"));
        }

        [Test]
        public void Expense_Should_SetAndGet_Date()
        {
            // Arrange
            var date = new DateTime(2025, 1, 1);
            var expense = new Expense
            {
                Date = date
            };

            // Assert
            Assert.That(expense.Date, Is.Not.Default);
            Assert.That(expense.Date, Is.EqualTo(date));
        }

        [Test]
        public void Expense_Should_SetAndGet_CategoryId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expense = new Expense
            {
                CategoryId = id
            };

            // Assert
            Assert.That(expense.CategoryId, Is.Not.Default);
            Assert.That(expense.CategoryId, Is.EqualTo(id));
        }

        [Test]
        public void Expense_Should_SetAndGet_CreatedAt()
        {
            // Arrange
            var date = new DateTime(2025, 1, 1);
            var expense = new Expense
            {
                CreatedAt = date
            };

            // Assert
            Assert.That(expense.CreatedAt, Is.Not.Default);
            Assert.That(expense.CreatedAt, Is.EqualTo(date));
        }

        [Test]
        public void Expense_Should_Have_NullOrDefault_Values_When_NotSet()
        {
            // Arrange
            var expense = new Expense();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(expense.Id, Is.Default);
                Assert.That(expense.Description, Is.EqualTo(string.Empty));
                Assert.That(expense.Money.Amount, Is.Zero);
                Assert.That(expense.Money.Currency, Is.EqualTo("USD"));
                Assert.That(expense.Date, Is.Default);
                Assert.That(expense.CategoryId, Is.Default);
                Assert.That(expense.CreatedAt, Is.Default);
            }
        }

        [Test]
        public void Expense_Equality_Should_Be_Based_On_Values()
        {
            // Arrange
            var id = Guid.NewGuid();
            var money = new Money(amount: 1);
            var expense1 = new Expense { Id = id, Description = "A", Money = money, Date = DateTime.Today, CategoryId = id, CreatedAt = DateTime.Today };
            var expense2 = new Expense { Id = id, Description = "A", Money = money, Date = DateTime.Today, CategoryId = id, CreatedAt = DateTime.Today };

            //Assert
            Assert.That(expense1, Is.EqualTo(expense2));
        }

        [Test]
        public void Expense_Should_Return_New_Instance_When_Using_With_Expression()
        {
            // Arrange
            var expense = new Expense { Description = "Old" };

            // Act
            var updated = expense with { Description = "New" };

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(updated.Description, Is.EqualTo("New"));
                Assert.That(expense.Description, Is.EqualTo("Old"));
            }
        }
    }
}
