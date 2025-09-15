using ExpenseTracker.Domain.Exceptions;

namespace ExpenseTracker.Domain.Tests.Exceptions
{
    [TestFixture]
    public class CategoryNotFoundExceptionTests
    {
        [Test]
        public void CategoryNotFoundException_Should_Inherit_From_DomainException()
        {
            // Arrange & Act
            var ex = new CategoryNotFoundException("Category not found.");

            // Assert
            Assert.That(ex, Is.InstanceOf<DomainException>());
        }

        [Test]
        public void CategoryNotFoundException_Should_Set_Message()
        {
            // Arrange & Act
            var message = "Category not found.";
            var ex = new CategoryNotFoundException(message);

            // Assert
            Assert.That(ex.Message, Is.EqualTo(message));
        }

        [Test]
        public void CategoryNotFoundException_Should_Be_Throwable()
        {
            // Arrange
            var message = "Category not found.";

            // Act & Assert
            var ex = Assert.Throws<CategoryNotFoundException>(() => throw new CategoryNotFoundException(message));
            Assert.That(ex.Message, Is.EqualTo(message));
        }
    }
}
