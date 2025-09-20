using ExpenseTracker.Infrastructure.Data;
using ExpenseTracker.Infrastructure.Data.Providers.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Serilog;
using System.Data;
using System.Data.Common;

namespace ExpenseTracker.Infrastructure.Tests.Data
{
    [TestFixture]
    public class DbConnectionFactoryTests
    {
        private Dictionary<string, IDbConnectionStrategy> _strategies;
        private ILogger _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            _strategies = [];
        }

        [Test]
        public async Task CreateAndOpenConnectionAsync_ReturnsOpenConnection_ForSupportedProvider()
        {
            // Arrange
            var fakeConnection = Substitute.For<DbConnection>();
            fakeConnection.When(x => x.OpenAsync(Arg.Any<CancellationToken>())).DoNotCallBase();
            fakeConnection.State.Returns(ConnectionState.Open);

            var strategy = Substitute.For<IDbConnectionStrategy>();
            strategy.CreateConnection(Arg.Any<string>()).Returns(fakeConnection);

            _strategies["postgres"] = strategy;

            var factory = new DbConnectionFactory(_strategies, _logger);

            // Act
            var result = await factory.CreateAndOpenConnectionAsync("postgres", "fake-conn", CancellationToken.None);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Is.EqualTo(fakeConnection));
                Assert.That(result.State, Is.EqualTo(ConnectionState.Open));
            }
        }

        [Test]
        public void CreateAndOpenConnectionAsync_ThrowsArgumentException_WhenProviderIsEmpty()
        {
            // Arrange
            var factory = new DbConnectionFactory(_strategies, _logger);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() =>
                factory.CreateAndOpenConnectionAsync(string.Empty, "conn", CancellationToken.None));
            Assert.That(ex.ParamName, Is.EqualTo("provider"));
        }

        [Test]
        public void CreateAndOpenConnectionAsync_ThrowsArgumentException_WhenProviderIsNull()
        {
            // Arrange
            var factory = new DbConnectionFactory(_strategies, _logger);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() =>
                factory.CreateAndOpenConnectionAsync(null!, "conn", CancellationToken.None));
            Assert.That(ex.ParamName, Is.EqualTo("provider"));
        }

        [Test]
        public void CreateAndOpenConnectionAsync_ThrowsNotSupportedException_ForUnsupportedProvider()
        {
            // Arrange
            var factory = new DbConnectionFactory(_strategies, _logger);

            // Act & Assert
            Assert.ThrowsAsync<NotSupportedException>(() => factory.CreateAndOpenConnectionAsync("oracle", "conn", CancellationToken.None));
        }

        [Test]
        public void CreateAndOpenConnectionAsync_ThrowsException_WhenStrategyThrows()
        {
            // Arrange
            var strategy = Substitute.For<IDbConnectionStrategy>();
            strategy.CreateConnection(Arg.Any<string>()).Throws(new InvalidOperationException("fail"));
            _strategies["mysql"] = strategy;

            // Act & Assert
            var factory = new DbConnectionFactory(_strategies, _logger);

            Assert.ThrowsAsync<InvalidOperationException>(() =>
                factory.CreateAndOpenConnectionAsync("mysql", "conn", CancellationToken.None));
        }

        [Test]
        public void CreateAndOpenConnectionAsync_RespectsCancellationToken()
        {
            // Arrange
            var fakeConnection = Substitute.For<DbConnection>();
            fakeConnection.When(x => x.OpenAsync(Arg.Any<CancellationToken>()))
                .Do(x => throw new OperationCanceledException());
            var strategy = Substitute.For<IDbConnectionStrategy>();
            strategy.CreateConnection(Arg.Any<string>()).Returns(fakeConnection);
            _strategies["postgres"] = strategy;

            // Act & Assert
            var factory = new DbConnectionFactory(_strategies, _logger);

            Assert.ThrowsAsync<OperationCanceledException>(() =>
                factory.CreateAndOpenConnectionAsync("postgres", "conn", new CancellationToken(true)));
        }
    }
}

