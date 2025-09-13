using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Handlers;
using NSubstitute;
using Serilog;

namespace ExpenseTracker.CLI.Tests.Handlers
{
    [TestFixture]
    public class ListCommandHandlerTests
    {
        private ILogger _logger;
        private ListCommand _command;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            _command = new ListCommand()
            {
                Category = "Food",
                Limit = 10,
                Month = 1,
                Year = 2025
            };
        }

        [Test]
        public async Task RunListAsync_AllDefaults_LogsInfoAndReturnsZero()
        {
            // Arrange
            var handler = new ListCommandHandler(_logger);

            // Act
            var result = await handler.RunListAsync(new ListCommand());

            // Assert
            _logger.Received(1).Information(Arg.Is<string>(s => s.Contains("Listing expenses")));
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RunListAsync_ValidMonthYearLimit_LogsInfoAndReturnsZero()
        {
            // Arrange
            var handler = new ListCommandHandler(_logger);

            // Act
            var result = await handler.RunListAsync(_command);

            // Assert
            _logger.Received(1).Information(Arg.Is<string>(
                s => s.Contains("Year:") && s.Contains("Month:")));
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RunListAsync_CategoryProvided_LogsInfoWithCategoryAndReturnsZero()
        {
            // Arrange
            var handler = new ListCommandHandler(_logger);

            // Act
            var result = await handler.RunListAsync(_command);

            // Assert
            _logger.Received(1).Information(Arg.Is<string>(s => s.Contains("Category:")));
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RunListAsync_InvalidMonth_LessThanOne_LogsErrorAndReturnsOne()
        {
            // Arrange
            _command.Month = 0;
            var handler = new ListCommandHandler(_logger);

            // Act
            var result = await handler.RunListAsync(_command);

            // Assert
            _logger.Received(1).Error("Month must be between 1 and 12.");
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task RunListAsync_InvalidMonth_GreaterThanTwelve_LogsErrorAndReturnsOne()
        {
            // Arrange
            _command.Month = 13;
            var handler = new ListCommandHandler(_logger);

            // Act
            var result = await handler.RunListAsync(_command);

            // Assert
            _logger.Received(1).Error("Month must be between 1 and 12.");
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task RunListAsync_InvalidYear_LessThan2025_LogsErrorAndReturnsOne()
        {
            // Arrange
            _command.Year = 2024;
            var handler = new ListCommandHandler(_logger);

            // Act
            var result = await handler.RunListAsync(_command);

            // Assert
            _logger.Received(1).Error("Year must be greater or equal than 2025.");
            Assert.That(result, Is.EqualTo(1));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task RunListAsync_InvalidLimit_ZeroOrNegative_LogsErrorAndReturnsOne(int limit)
        {
            // Arrange
            _command.Limit = limit;
            var handler = new ListCommandHandler(_logger);

            // Act
            var result = await handler.RunListAsync(_command);

            // Assert
            _logger.Received(1).Error("Limit must be greater than 0.");
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void RunListAsync_NullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new ListCommandHandler(null!));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("logger"));
                Assert.That(ex.Message, Does.Contain("Logger cannot be null."));
            }
        }
    }
}
