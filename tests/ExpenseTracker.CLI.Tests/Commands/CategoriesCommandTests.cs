using CommandLine;
using ExpenseTracker.CLI.Commands;

namespace ExpenseTracker.CLI.Tests.Commands
{
    [TestFixture]
    public class CategoriesCommandTests
    {
        [Test]
        public void CategoriesCommand_Parses_ListAction_WithNoOptions_Success()
        {
            // Arrange
            var listActionWithNoOptions = new string[] { "categories", "-a", "list" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(listActionWithNoOptions);

            // Assert
            Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
        }

        [Test]
        public void CategoriesCommand_Parses_ListAction_WithShortName_Success()
        {
            // Arrange
            var listActionWithShortName = new string[] { "categories", "-a", "list", "-n", "Transport" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(listActionWithShortName);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Name, Is.EqualTo("Transport"));
            }
        }

        [Test]
        public void CategoriesCommand_Parses_ListAction_WithLongName_Success()
        {
            // Arrange
            var listActionWithLongName = new string[] { "categories", "-a", "list", "--name", "Transport" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(listActionWithLongName);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Name, Is.EqualTo("Transport"));
            }
        }

        [Test]
        public void CategoriesCommand_Parses_ListAction_WithShortId_Success()
        {
            // Arrange
            var listActionWithShortId = new string[] { "categories", "-a", "list", "-i", "1" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(listActionWithShortId);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Id, Is.EqualTo(1));
            }
        }

        [Test]
        public void CategoriesCommand_Parses_ListAction_WithLongId_Success()
        {
            // Arrange
            var listActionWithLongId = new string[] { "categories", "-a", "list", "--id", "3" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(listActionWithLongId);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Id, Is.EqualTo(3));
            }
        }

        [Test]
        public void CategoriesCommand_Parses_AddAction_WithNoOptions_Success()
        {
            // Arrange
            var addActionWithNoOptions = new string[] { "categories", "-a", "add" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(addActionWithNoOptions);

            // Assert
            Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
        }

        [Test]
        public void CategoriesCommand_Parses_AddAction_WithShortName_Success()
        {
            // Arrange
            var addActionWithShortName = new string[] { "categories", "-a", "add", "-n", "Transport" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(addActionWithShortName);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Name, Is.EqualTo("Transport"));
            }
        }

        [Test]
        public void CategoriesCommand_Parses_AddAction_WithLongName_Success()
        {
            // Arrange
            var addActionWithLongName = new string[] { "categories", "-a", "add", "--name", "Transport" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(addActionWithLongName);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Name, Is.EqualTo("Transport"));
            }
        }

        [Test]
        public void CategoriesCommand_Parses_AddAction_WithShortId_Success()
        {
            // Arrange
            var addActionWithShortId = new string[] { "categories", "-a", "add", "-i", "1" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(addActionWithShortId);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Id, Is.EqualTo(1));
            }
        }

        [Test]
        public void CategoriesCommand_Parses_AddAction_WithLongId_Success()
        {
            // Arrange
            var addActionWithLongId = new string[] { "categories", "-a", "add", "--id", "3" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(addActionWithLongId);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Id, Is.EqualTo(3));
            }
        }

        [Test]
        public void CategoriesCommand_Parses_DeleteAction_WithNoOptions_Success()
        {
            // Arrange
            var deleteActionWithNoOptions = new string[] { "categories", "-a", "delete" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(deleteActionWithNoOptions);

            // Assert
            Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
        }

        [Test]
        public void CategoriesCommand_Parses_DeleteAction_WithShortName_Success()
        {
            // Arrange
            var deleteActionWithShortName = new string[] { "categories", "-a", "add", "-n", "Transport" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(deleteActionWithShortName);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Name, Is.EqualTo("Transport"));
            }
        }

        [Test]
        public void CategoriesCommand_Parses_DeleteAction_WithLongName_Success()
        {
            // Arrange
            var deleteActionWithLongName = new string[] { "categories", "-a", "add", "--name", "Transport" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(deleteActionWithLongName);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Name, Is.EqualTo("Transport"));
            }
        }

        [Test]
        public void CategoriesCommand_Parses_DeleteAction_WithShortId_Success()
        {
            // Arrange
            var deleteActionWithShortId = new string[] { "categories", "-a", "add", "-i", "1" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(deleteActionWithShortId);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Id, Is.EqualTo(1));
            }
        }

        [Test]
        public void CategoriesCommand_Parses_DeleteAction_WithLongId_Success()
        {
            // Arrange
            var deleteActionWithLongId = new string[] { "categories", "-a", "add", "--id", "3" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(deleteActionWithLongId);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Id, Is.EqualTo(3));
            }
        }

        [Test]
        public void CategoriesCommand_MissingAction_Fails()
        {
            // Arrange
            var noActionSpecified = new string[] { "categories" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(noActionSpecified);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.NotParsed));
                Assert.That(result.Errors.OfType<MissingRequiredOptionError>()
                    .Any(e => e.NameInfo.LongName == "action" && e.NameInfo.ShortName == "a"), Is.True);
            }
        }

        [Test]
        public void CategoriesCommand_InvalidAction_Fails()
        {
            // Arrange
            var withInvalidAction = new string[] { "categories", "-a", "update" };

            // Act
            var result = Parser.Default.ParseArguments<CategoriesCommand>(withInvalidAction);

            using (Assert.EnterMultipleScope())
            {
                // Assert
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.NotParsed));
            }
        }
    }
}
