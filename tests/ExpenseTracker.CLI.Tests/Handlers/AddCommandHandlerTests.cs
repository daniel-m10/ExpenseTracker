using ExpenseTracker.CLI.Abstractions;
using ExpenseTracker.CLI.Commands;
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
            _logger.Received(5).Information(Arg.Any<string>());
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
            _logger.Received(1).Error(Arg.Is<string>(s => s.Contains("Amount must be greater or equal to 0.")));
        }

        [Test]
        public async Task RunAddAsync_InvalidDateFormat_LogsErrorAndReturnsOne()
        {
            // Arrange
            var handler = new AddCommandHandler(_logger, _dateParser);

            // Act
            var result = await handler.RunAddAsync(new AddCommand());

            // Assert
            _logger.Received(2).Error(Arg.Any<string>());
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
                Assert.That(ex.Message, Does.Contain("Logger cannot be null."));
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
                Assert.That(ex.Message, Does.Contain("Date parser cannot be null."));
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
            _logger.Received(5).Information(Arg.Any<string>());

            var calls = _logger.ReceivedCalls()
                .Where(call => call.GetMethodInfo().Name == "Information")
                .Select(call => call.GetArguments()[0] as string)
                .ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Is.Zero);
                Assert.That(calls[0], Does.Contain("Expense recorded"));
                Assert.That(calls[1], Does.Contain("Description"));
                Assert.That(calls[2], Does.Contain("Amount"));
                Assert.That(calls[3], Does.Contain("Category"));
                Assert.That(calls[4], Does.Contain("Date"));
            }
        }

        [Test]
        public async Task RunAddAsync_InvalidDateFormat_LogsDateFormatHint()
        {
            // Arrange
            var handler = new AddCommandHandler(_logger, _dateParser);

            // Act
            var result = await handler.RunAddAsync(new AddCommand());

            // Assert
            _logger.Received(2).Error(Arg.Any<string>());

            var calls = _logger.ReceivedCalls()
                .Where(call => call.GetMethodInfo().Name == "Error")
                .Select(call => call.GetArguments()[0] as string)
                .ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Is.EqualTo(1));
                Assert.That(calls[0], Does.Contain("Wrong data format for Expense date:"));
                Assert.That(calls[1], Does.Contain("Please use this format:"));
            }
        }
    }
}
