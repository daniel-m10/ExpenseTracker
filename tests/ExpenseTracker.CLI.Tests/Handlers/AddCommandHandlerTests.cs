using ExpenseTracker.CLI.Abstractions;
using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Constants;
using ExpenseTracker.CLI.Handlers;
using NSubstitute;
using Serilog;
using System.Globalization;

namespace ExpenseTracker.CLI.Tests.Handlers
{
    [TestFixture]
    public class AddCommandHandlerTests
    {
        private ILogger _logger;
        private IDateParser _dateParser;
        private AddCommand _command;

        [SetUp]
        public void SetUp()
        {
            _dateParser = Substitute.For<IDateParser>();
            _logger = Substitute.For<ILogger>();
            _command = new AddCommand
            {
                Description = "Test",
                Amount = 10,
                Category = "Food",
                Date = "2025-01-01"
            };
        }

        [Test]
        public async Task RunAddAsync_ValidCommand_LogsSuccessAndReturnsZero()
        {
            // Arrange
            var parsedDate = new DateTime(2025, 1, 1);

            _dateParser.TryParseExact(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IFormatProvider>(),
                Arg.Any<DateTimeStyles>(),
                out Arg.Any<DateTime>()).Returns(x =>
                {
                    x[4] = parsedDate;
                    return true;
                });

            var handler = new AddCommandHandler(_logger, _dateParser);

            // Act
            var result = await handler.RunAddAsync(_command);

            // Assert
            _logger.Received(1).Information(Messages.ExpenseRecordedSuccessfully);
            _logger.Received(1).Information(Messages.DescriptionLabel, "Test");
            _logger.Received(1).Information(Messages.AmountLabel, 10m);
            _logger.Received(1).Information(Messages.CategoryLabel, "Food");
            _logger.Received(1).Information(Messages.DateLabel, Arg.Any<DateTime>());
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RunAddAsync_NegativeAmount_LogsErrorAndReturnsOne()
        {
            // Arrange
            _command.Amount = -10;

            var handler = new AddCommandHandler(_logger, _dateParser);

            // Act
            var result = await handler.RunAddAsync(_command);

            // Assert
            Assert.That(result, Is.EqualTo(1));
            _logger.Received(1).Error(Messages.AmountMustBeNonNegative);
        }

        [Test]
        public async Task RunAddAsync_InvalidDateFormat_LogsErrorAndReturnsOne()
        {
            // Arrange
            _dateParser.TryParseExact(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IFormatProvider>(),
                Arg.Any<DateTimeStyles>(),
                out Arg.Any<DateTime>()).Returns(false);

            var handler = new AddCommandHandler(_logger, _dateParser);

            // Act
            var result = await handler.RunAddAsync(new AddCommand());

            // Assert
            _logger.Received(1).Error(Messages.WrongDateFormat, Arg.Any<string>());
            _logger.Received(1).Error(Messages.DateFormatHint);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void RunAddAsync_NullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new AddCommandHandler(null!, _dateParser));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("logger"));
                Assert.That(ex.Message, Does.Contain(Messages.LoggerCannotBeNull));
            }
        }

        [Test]
        public void RunAddAsync_NullDateParser_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new AddCommandHandler(_logger, null!));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("dateParser"));
                Assert.That(ex.Message, Does.Contain(Messages.DateParserCannotBeNull));
            }
        }

        [Test]
        public async Task RunAddAsync_ValidCommand_LogsAllExpenseFields()
        {
            // Arrange
            var parsedDate = new DateTime(2025, 1, 1);

            _dateParser.TryParseExact(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IFormatProvider>(),
                Arg.Any<DateTimeStyles>(),
                out Arg.Any<DateTime>()).Returns(x =>
                {
                    x[4] = parsedDate;
                    return true;
                });

            var handler = new AddCommandHandler(_logger, _dateParser);

            // Act
            var result = await handler.RunAddAsync(_command);

            // Assert

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Is.Zero);
                _logger.Received(1).Information(Messages.ExpenseRecordedSuccessfully);
                _logger.Received(1).Information(Messages.DescriptionLabel, "Test");
                _logger.Received(1).Information(Messages.AmountLabel, 10m);
                _logger.Received(1).Information(Messages.CategoryLabel, "Food");
                _logger.Received(1).Information(Messages.DateLabel, parsedDate);
            }
        }

        [Test]
        public async Task RunAddAsync_InvalidDateFormat_LogsDateFormatHint()
        {
            // Arrange
            _dateParser.TryParseExact(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IFormatProvider>(),
                Arg.Any<DateTimeStyles>(),
                out Arg.Any<DateTime>()).Returns(false);

            var handler = new AddCommandHandler(_logger, _dateParser);

            // Act
            var result = await handler.RunAddAsync(new AddCommand());

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Is.EqualTo(1));
                _logger.Received(1).Error(Messages.WrongDateFormat, Arg.Any<string>());
                _logger.Received(1).Error(Messages.DateFormatHint);
            }
        }
    }
}
