using ExpenseTracker.CLI.Commands;
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
            _logger.Received(5).Information(Arg.Any<string>());
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
            _logger.Received(1).Error("Id must be greater than 0.");
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
                Assert.That(ex.Message, Does.Contain("Logger cannot be null"));
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
            _logger.Received(5).Information(Arg.Any<string>());

            var calls = _logger.ReceivedCalls()
                .Where(call => call.GetMethodInfo().Name == "Information")
                .Select(call => call.GetArguments()[0] as string)
                .ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Is.Zero);
                Assert.That(calls[0], Does.Contain("Showing details for expense"));
                Assert.That(calls[1], Does.Contain("Description"));
                Assert.That(calls[2], Does.Contain("Amount"));
                Assert.That(calls[3], Does.Contain("Category"));
                Assert.That(calls[4], Does.Contain("Date"));
            }
        }
    }
}
