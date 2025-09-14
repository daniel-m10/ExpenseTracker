using ExpenseTracker.CLI.Abstractions;
using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Constants;
using ExpenseTracker.CLI.Handlers;
using NSubstitute;
using Serilog;

namespace ExpenseTracker.CLI.Tests.Handlers
{
    [TestFixture]
    public class DeleteCommandHandlerTests
    {
        private ILogger _logger;
        private IConsoleInput _input;
        private DeleteCommand _command;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            _input = Substitute.For<IConsoleInput>();
            _command = new DeleteCommand
            {
                Id = 1,
                Force = false
            };
        }

        [Test]
        public async Task RunDeleteAsync_ValidId_LogsSuccessAndReturnsZero()
        {
            // Arrange
            _input.ReadLine().Returns("y");
            var handler = new DeleteCommandHandler(_logger, _input);

            // Act
            var result = await handler.RunDeleteAsync(_command);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Is.Zero);
                _logger.Received(1).Information(Messages.ConfirmDeletePrompt, 1);
                _logger.Received(1).Information(Messages.ExpenseDeletedSuccessfully, 1);
            }
        }

        [Test]
        public async Task RunDeleteAsync_ValidIdWithForce_DeletesWithoutConfirmation()
        {
            // Arrange
            _command.Force = true;
            var handler = new DeleteCommandHandler(_logger, _input);

            // Act
            var result = await handler.RunDeleteAsync(_command);

            // Assert
            Assert.That(result, Is.Zero);
            _logger.Received(1).Information(Messages.ExpenseDeletedSuccessfully, 1);
        }

        [Test]
        public async Task RunDeleteAsync_InvalidId_LogsErrorAndReturnsOne()
        {
            // Arrange
            _command.Id = -1;
            var handler = new DeleteCommandHandler(_logger, _input);

            // Act
            var result = await handler.RunDeleteAsync(_command);

            // Assert
            Assert.That(result, Is.EqualTo(1));
            _logger.Received(1).Error(Messages.IdMustBeGreaterThanZero);
        }

        [Test]
        public void RunDeleteAsync_NullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new DeleteCommandHandler(null!, _input));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("logger"));
                Assert.That(ex.Message, Does.Contain("Logger cannot be null."));
            }
        }

        [Test]
        public void RunDeleteAsync_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new DeleteCommandHandler(_logger, null!));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("input"));
                Assert.That(ex.Message, Does.Contain("Console Input cannot be null."));
            }
        }

        [Test]
        public async Task RunDeleteAsync_ConfirmationDeclined_LogsAndReturnsOne()
        {
            // Arrange
            _input.ReadLine().Returns("n");
            var handler = new DeleteCommandHandler(_logger, _input);

            // Act
            var result = await handler.RunDeleteAsync(_command);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Is.EqualTo(1));
                _logger.Received(1).Information(Messages.ConfirmDeletePrompt, 1);
                _logger.Received(1).Information(Messages.DeleteCancelled);
            }
        }
    }
}
