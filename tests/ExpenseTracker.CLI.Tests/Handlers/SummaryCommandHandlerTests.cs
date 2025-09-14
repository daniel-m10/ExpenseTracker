using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Constants;
using ExpenseTracker.CLI.Handlers;
using NSubstitute;
using Serilog;

namespace ExpenseTracker.CLI.Tests.Handlers
{
    [TestFixture]
    public class SummaryCommandHandlerTests
    {
        private ILogger _logger;
        private SummaryCommand _command;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            _command = new SummaryCommand
            {
                Category = "Food",
                Month = 1,
                Year = 2025
            };
        }

        [Test]
        public async Task RunSummaryAsync_AllDefaults_LogsSummaryAndReturnsZero()
        {
            // Arrange
            _command.Category = string.Empty;
            var handler = new SummaryCommandHandler(_logger);

            // Act
            var result = await handler.RunSummaryAsync(_command);

            // Assert
            _logger.Received(1).Information(Messages.SummaryHeader, Arg.Any<int>(), Arg.Any<int>(), "(any)");
            _logger.Received(1).Information(Messages.TotalExpenses, Arg.Any<decimal>());
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RunSummaryAsync_ValidMonthYearCategory_LogsSummaryAndReturnsZero()
        {
            // Arrange
            var handler = new SummaryCommandHandler(_logger);

            // Act
            var result = await handler.RunSummaryAsync(_command);

            // Assert
            _logger.Received(1).Information(Messages.SummaryHeader, 2025, 1, "Food");
            _logger.Received(1).Information(Messages.TotalExpenses, Arg.Any<decimal>());
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RunSummaryAsync_CategoryMissing_LogsAnyCategory()
        {
            // Arrange
            _command.Category = string.Empty;
            var handler = new SummaryCommandHandler(_logger);

            // Act
            var result = await handler.RunSummaryAsync(_command);

            // Assert
            _logger.Received(1).Information(Messages.SummaryHeader, Arg.Any<int>(), Arg.Any<int>(), "(any)");
            _logger.Received(1).Information(Messages.TotalExpenses, Arg.Any<decimal>());
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RunSummaryAsync_InvalidMonth_LessThanOne_LogsErrorAndReturnsOne()
        {
            // Arrange
            _command.Month = 0;
            var handler = new SummaryCommandHandler(_logger);

            // Act
            var result = await handler.RunSummaryAsync(_command);

            // Assert
            _logger.Received(1).Error(Messages.MonthMustBeBetween1And12);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task RunSummaryAsync_InvalidMonth_GreaterThanTwelve_LogsErrorAndReturnsOne()
        {
            // Arrange
            _command.Month = 13;
            var handler = new SummaryCommandHandler(_logger);

            // Act
            var result = await handler.RunSummaryAsync(_command);

            // Assert
            _logger.Received(1).Error(Messages.MonthMustBeBetween1And12);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task RunSummaryAsync_InvalidYear_LessThan2025_LogsErrorAndReturnsOne()
        {
            // Arrange
            _command.Year = 2024;
            var handler = new SummaryCommandHandler(_logger);

            // Act
            var result = await handler.RunSummaryAsync(_command);

            // Assert
            _logger.Received(1).Error("Year must be greater or equal than 2025.");
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void RunSummaryAsync_NullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new SummaryCommandHandler(null!));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("logger"));
                Assert.That(ex.Message, Does.Contain(Messages.LoggerCannotBeNull));
            }
        }
    }
}
