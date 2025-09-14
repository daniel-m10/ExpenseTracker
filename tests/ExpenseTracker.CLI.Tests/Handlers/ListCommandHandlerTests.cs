using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Constants;
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
            _logger.Received(1).Information(Messages.ListingExpenses, Arg.Any<int>(), Arg.Any<int>(), "(any)", Arg.Any<int>());
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
            _logger.Received(1).Information(Messages.ListingExpenses, 2025, 1, "Food", 10);
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
            _logger.Received(1).Information(Messages.ListingExpenses, 2025, 1, "Food", 10);
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
            _logger.Received(1).Error(Messages.MonthMustBeBetween1And12);
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
            _logger.Received(1).Error(Messages.MonthMustBeBetween1And12);
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
            _logger.Received(1).Error(Messages.LimitMustBeGreaterThanZero);
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
                Assert.That(ex.Message, Does.Contain(Messages.LoggerCannotBeNull));
            }
        }
    }
}
