using ExpenseTracker.Domain.ValueObjects;

namespace ExpenseTracker.Domain.Tests.ValueObjects
{
    [TestFixture]
    public class MoneyTests
    {
        [Test]
        public void Money_Should_Create_With_Valid_Amount_And_Currency()
        {
            // Arrange & Act
            var money = new Money(amount: 10, currency: "BOB");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(money, Is.Not.Null);
                Assert.That(money.Amount, Is.EqualTo(10));
                Assert.That(money.Currency, Is.EqualTo("BOB"));
            }
        }

        [Test]
        public void Money_Should_Throw_When_Amount_Is_Negative()
        {
            // Act & Arrange
            var ex = Assert.Throws<ArgumentException>(() => new Money(amount: -1));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("amount"));
                Assert.That(ex.Message, Does.Contain("Amount must be non-negative."));
            }
        }

        [Test]
        public void Money_Should_Throw_When_Currency_Is_Null()
        {
            // Act & Arrange
            var ex = Assert.Throws<ArgumentException>(() => new Money(amount: 10, currency: null!));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("currency"));
                Assert.That(ex.Message, Does.Contain("Currency is required."));
            }
        }

        [Test]
        public void Money_Should_Throw_When_Currency_Is_Empty()
        {
            // Act & Arrange
            var ex = Assert.Throws<ArgumentException>(() => new Money(amount: 10, currency: string.Empty));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("currency"));
                Assert.That(ex.Message, Does.Contain("Currency is required."));
            }
        }

        [Test]
        public void Money_Should_Default_To_Uppercase_Currency()
        {
            // Arrange & Act
            var money = new Money(amount: 2);

            // Assert
            Assert.That(money.Currency, Is.EqualTo("USD"));
        }

        [Test]
        public void Money_Equality_Should_Be_Based_On_Amount_And_Currency()
        {
            // Arrange & Act
            var money1 = new Money(amount: 1);
            var money2 = new Money(amount: 1);

            // Assert
            Assert.That(money1, Is.EqualTo(money2));
        }

        [Test]
        public void Money_Addition_Should_Sum_Amounts_With_Same_Currency()
        {
            // Arrange
            var money1 = new Money(amount: 1);
            var money2 = new Money(amount: 2);

            // Act
            var result = money1 + money2;

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Amount, Is.EqualTo(3));
                Assert.That(result.Currency, Is.EqualTo("USD"));
                Assert.That(result, Is.Not.EqualTo(money1));
                Assert.That(result, Is.Not.EqualTo(money2));
            }
        }

        [Test]
        public void Money_Addition_Should_Throw_When_Currencies_Differ()
        {
            // Arrange
            var money1 = new Money(amount: 1);
            var money2 = new Money(amount: 2, currency: "BOB");

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => { var _ = money1 + money2; });
            Assert.That(ex.Message, Does.Contain("Cannot add Money with different currencies."));
        }

        [Test]
        public void Money_Subtraction_Should_Subtract_Amounts_With_Same_Currency()
        {
            // Arrange
            var money1 = new Money(amount: 7);
            var money2 = new Money(amount: 2);

            // Act
            var result = money1 - money2;

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Amount, Is.EqualTo(5));
                Assert.That(result.Currency, Is.EqualTo("USD"));
                Assert.That(result, Is.Not.EqualTo(money1));
                Assert.That(result, Is.Not.EqualTo(money2));
            }
        }

        [Test]
        public void Money_Subtraction_Should_Throw_When_Currencies_Differ()
        {
            // Arrange
            var money1 = new Money(amount: 7);
            var money2 = new Money(amount: 5, currency: "BOB");

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => { var _ = money1 - money2; });
            Assert.That(ex.Message, Does.Contain("Cannot subtract Money with different currencies."));
        }

        [Test]
        public void Money_ToString_Should_Return_Formatted_String()
        {
            // Act & Assert
            var money = new Money(amount: 7);

            Assert.That(money.ToString(), Is.EqualTo("7.00 USD"));
        }
    }
}
