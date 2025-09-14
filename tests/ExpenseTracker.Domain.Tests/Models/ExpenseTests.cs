using ExpenseTracker.Domain.Models;

namespace ExpenseTracker.Domain.Tests.Models
{
    [TestFixture]
    public class ExpenseTests
    {
        private Expense _expense;

        [SetUp]
        public void SetUp()
        {
            _expense = new Expense();
        }

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
            var amount = 2m;
            _expense.Amount = amount;

            // Assert
            Assert.That(_expense.Amount, Is.Not.Default);
            Assert.That(_expense.Amount, Is.EqualTo(amount));
        }

        [Test]
        public void Expense_Should_SetAndGet_Date()
        {
            // Arrange
            var date = new DateTime(2025, 1, 1);
            _expense.Date = date;

            // Assert
            Assert.That(_expense.Date, Is.Not.Default);
            Assert.That(_expense.Date, Is.EqualTo(date));
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
            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_expense.Id, Is.Default);
                Assert.That(_expense.Description, Is.EqualTo(string.Empty));
                Assert.That(_expense.Amount, Is.Default);
                Assert.That(_expense.Date, Is.Default);
                Assert.That(_expense.CategoryId, Is.Default);
                Assert.That(_expense.CreatedAt, Is.Default);
            }
        }
    }
}
