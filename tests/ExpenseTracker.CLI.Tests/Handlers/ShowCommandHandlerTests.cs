using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Constants;
using ExpenseTracker.CLI.Handlers;
using NSubstitute;
using Serilog;

namespace ExpenseTracker.CLI.Tests.Handlers
{
    [TestFixture]
    public class ShowCommandHandlerTests
    {
        private ILogger _logger;
        private ShowCommand _command;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger>();
            _command = new ShowCommand()
            {
                Id = 1
            };
        }

        [Test]
        public async Task RunShowAsync_ValidId_LogsExpenseDetailsAndReturnsZero()
        {
            // Arrange
            var handler = new ShowCommandHandler(_logger);

            // Act
            var result = await handler.RunShowAsync(_command);

            // Assert
            _logger.Received(1).Information(Messages.ShowingDetailsForExpense, 1);
            _logger.Received(1).Information(Messages.DescriptionLabel, "Lunch");
            _logger.Received(1).Information(Messages.AmountLabel, 20.00m);
            _logger.Received(1).Information(Messages.CategoryLabel, "Food");
            _logger.Received(1).Information(Messages.DateLabel, Arg.Any<DateTime>());
            Assert.That(result, Is.Zero);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task RunShowAsync_IdLessThanOrEqualToZero_LogsErrorAndReturnsOne(int id)
        {
            // Arrange
            _command.Id = id;
            var handler = new ShowCommandHandler(_logger);

            // Act
            var result = await handler.RunShowAsync(_command);

            // Assert
            _logger.Received(1).Error(Messages.IdMustBeGreaterThanZero);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void RunShowAsync_NullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new ShowCommandHandler(null!));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("logger"));
                Assert.That(ex.Message, Does.Contain(Messages.LoggerCannotBeNull));
            }
        }

        [Test]
        public async Task RunShowAsync_ValidId_LogsAllExpenseFields()
        {
            // Arrange
            var handler = new ShowCommandHandler(_logger);

            // Act
            var result = await handler.RunShowAsync(_command);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Is.Zero);
                _logger.Received(1).Information(Messages.ShowingDetailsForExpense, 1);
                _logger.Received(1).Information(Messages.DescriptionLabel, "Lunch");
                _logger.Received(1).Information(Messages.AmountLabel, 20.00m);
                _logger.Received(1).Information(Messages.CategoryLabel, "Food");
                _logger.Received(1).Information(Messages.DateLabel, Arg.Any<DateTime>());
            }
        }
    }
}
