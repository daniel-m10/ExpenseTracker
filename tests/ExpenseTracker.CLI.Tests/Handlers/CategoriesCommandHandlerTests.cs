using ExpenseTracker.CLI.Commands;
using ExpenseTracker.CLI.Handlers;
using ExpenseTracker.CLI.Utils;
using NSubstitute;
using Serilog;

namespace ExpenseTracker.CLI.Tests.Handlers
{
    [TestFixture]
    public class CategoriesCommandHandlerTests
    {
        private ILogger _logger;
        private CategoriesCommand _command;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            _command = new CategoriesCommand
            {
                Action = CategoryAction.list,
                Id = 1,
                Name = "Food"
            };
        }

        [Test]
        public async Task RunCategoriesAsync_ListAction_LogsCategoriesAndReturnsZero()
        {
            // Arrange
            var handler = new CategoriesCommandHandler(_logger);

            // Act
            var result = await handler.RunCategoriesAsync(_command);

            // Assert
            _logger.Received(1).Information(Arg.Is<string>(s => s.Contains("Categories")));
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RunCategoriesAsync_AddAction_ValidName_LogsSuccessAndReturnsZero()
        {
            // Arrange
            _command.Action = CategoryAction.add;
            var handler = new CategoriesCommandHandler(_logger);

            // Act
            var result = await handler.RunCategoriesAsync(_command);

            // Assert
            _logger.Received(1).Information(Arg.Is<string>(s => s.Contains("Category added")));
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RunCategoriesAsync_DeleteAction_ValidId_LogsSuccessAndReturnsZero()
        {
            // Arrange
            _command.Action = CategoryAction.delete;
            _command.Name = string.Empty;
            var handler = new CategoriesCommandHandler(_logger);

            // Act
            var result = await handler.RunCategoriesAsync(_command);

            // Assert
            _logger.Received(1).Information(Arg.Is<string>(s => s.Contains("Category deleted (id")));
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RunCategoriesAsync_DeleteAction_ValidName_LogsSuccessAndReturnsZero()
        {
            // Arrange
            _command.Action = CategoryAction.delete;
            _command.Id = 0;
            var handler = new CategoriesCommandHandler(_logger);

            // Act
            var result = await handler.RunCategoriesAsync(_command);

            // Assert
            _logger.Received(1).Information(Arg.Is<string>(s => s.Contains("Category deleted (name")));
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RunCategoriesAsync_AddAction_MissingName_LogsErrorAndReturnsOne()
        {
            // Arrange
            _command.Action = CategoryAction.add;
            _command.Name = string.Empty;
            var handler = new CategoriesCommandHandler(_logger);

            // Act
            var result = await handler.RunCategoriesAsync(_command);

            // Assert
            _logger.Received(1).Error(Arg.Is<string>(s => s.Contains("Missing --name for 'add'")));
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task RunCategoriesAsync_DeleteAction_MissingIdAndName_LogsErrorAndReturnsOne()
        {
            // Arrange
            _command.Action = CategoryAction.delete;
            _command.Name = string.Empty;
            _command.Id = 0;
            var handler = new CategoriesCommandHandler(_logger);

            // Act
            var result = await handler.RunCategoriesAsync(_command);

            // Assert
            _logger.Received(1).Error(Arg.Is<string>(s => s.Contains("For 'delete', provide exactly one: --id ID  OR  --name NAME")));
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task RunCategoriesAsync_InvalidAction_LogsErrorAndReturnsOne()
        {
            // Arrange
            _command.Action = CategoryAction.none;
            var handler = new CategoriesCommandHandler(_logger);

            // Act
            var result = await handler.RunCategoriesAsync(_command);

            // Assert
            _logger.Received(1).Error(Arg.Is<string>(s => s.Contains("Invalid ACTION. Use: list | add | delete")));
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void RunCategoriesAsync_NullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new CategoriesCommandHandler(null!));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.ParamName, Is.EqualTo("logger"));
                Assert.That(ex.Message, Does.Contain("Logger cannot be null"));
            }
        }
    }
}
