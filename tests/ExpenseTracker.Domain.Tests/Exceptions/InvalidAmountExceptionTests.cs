using ExpenseTracker.Domain.Exceptions;

namespace ExpenseTracker.Domain.Tests.Exceptions
{
    [TestFixture]
    public class InvalidAmountExceptionTests
    {
        [Test]
        public void InvalidAmountException_Should_Inherit_From_DomainException()
        {
            // Arrange & Act
            var ex = new InvalidAmountException("Invalid amount.");

            // Assert
            Assert.That(ex, Is.InstanceOf<DomainException>());
        }

        [Test]
        public void InvalidAmountException_Should_Set_Message()
        {
            // Arrange & Act
            var message = "Invalid amount.";
            var ex = new InvalidAmountException(message);

            // Assert
            Assert.That(ex.Message, Is.EqualTo(message));
        }

        [Test]
        public void InvalidAmountException_Should_Be_Throwable()
        {
            // Arrange
            var message = "Invalid amount.";

            // Act & Assert
            var ex = Assert.Throws<InvalidAmountException>(() => throw new InvalidAmountException(message));
            Assert.That(ex.Message, Is.EqualTo(message));
        }
    }
}
