using ExpenseTracker.Domain.Exceptions;

namespace ExpenseTracker.Domain.Tests.Exceptions
{
    [TestFixture]
    public class ExpenseNotFoundExceptionTests
    {
        [Test]
        public void ExpenseNotFoundException_Should_Inherit_From_DomainException()
        {
            // Arrange & Act
            var ex = new ExpenseNotFoundException("Expense not found.");

            // Assert
            Assert.That(ex, Is.InstanceOf<DomainException>());
        }

        [Test]
        public void ExpenseNotFoundException_Should_Set_Message()
        {
            // Arrange & Act
            var message = "Expense not found.";
            var ex = new ExpenseNotFoundException(message);

            // Assert
            Assert.That(ex.Message, Is.EqualTo(message));
        }

        [Test]
        public void ExpenseNotFoundException_Should_Be_Throwable()
        {
            // Arrange
            var message = "Expense not found.";

            // Act & Assert
            var ex = Assert.Throws<ExpenseNotFoundException>(() => throw new ExpenseNotFoundException(message));
            Assert.That(ex.Message, Is.EqualTo(message));
        }
    }
}
