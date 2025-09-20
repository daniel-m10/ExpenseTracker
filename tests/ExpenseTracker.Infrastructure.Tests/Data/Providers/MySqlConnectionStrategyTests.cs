using ExpenseTracker.Infrastructure.Data.Providers;
using ExpenseTracker.Infrastructure.Data.Providers.Interfaces;

namespace ExpenseTracker.Infrastructure.Tests.Data.Providers
{
    [TestFixture]
    public class MySqlConnectionStrategyTests
    {
        private IDbConnectionStrategy _connectionStrategy;

        [SetUp]
        public void SetUp()
        {
            _connectionStrategy = new MySqlConnectionStrategy();
        }

        [Test]
        public void CreateConnection_ReturnsMySqlConnection_WhenConnectionStringIsValid()
        {
            // Arrange
            string connStr = "Server=localhost;Database=expense_tracker;User=root;Password=your_password;";

            // Act
            var result = _connectionStrategy.CreateConnection(connStr);

            // Assert
            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.GetType().Name, Is.EqualTo("MySqlConnection"));
                Assert.That(result.ConnectionString, Is.EqualTo(connStr));
                Assert.That(result.State, Is.EqualTo(System.Data.ConnectionState.Closed));
            }
        }

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenConnectionStringIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _connectionStrategy.CreateConnection(null!));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("connectionString"));
                Assert.That(ex.Message, Does.Contain("Connection string cannot be null or empty."));
            }
        }

        [Test]
        public void CreateConnection_ThrowsArgumentException_WhenConnectionStringIsEmpty()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _connectionStrategy.CreateConnection(string.Empty));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("connectionString"));
                Assert.That(ex.Message, Does.Contain("Connection string cannot be null or empty."));
            }
        }
    }
}
