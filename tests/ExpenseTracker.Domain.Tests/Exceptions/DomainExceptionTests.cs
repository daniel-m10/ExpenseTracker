using ExpenseTracker.Domain.Exceptions;

namespace ExpenseTracker.Domain.Tests.Exceptions
{
    [TestFixture]
    public class DomainExceptionTests
    {
        [Test]
        public void DomainException_Should_Set_Message()
        {
            // Arrange & Act
            var message = "Something went wrong at Domain level.";
            var ex = new DomainException(message);

            // Assert
            Assert.That(ex.Message, Is.EqualTo(message));
        }

        [Test]
        public void DomainException_Should_Be_Throwable()
        {
            // Arrange
            var message = "Something went wrong at Domain level.";

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => throw new DomainException(message));
            Assert.That(ex.Message, Is.EqualTo(message));
        }
    }
}
