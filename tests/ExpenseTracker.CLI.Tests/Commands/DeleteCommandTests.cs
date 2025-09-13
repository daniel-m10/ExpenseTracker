using CommandLine;
using ExpenseTracker.CLI.Commands;

namespace ExpenseTracker.CLI.Tests.Commands
{
    [TestFixture]
    public class DeleteCommandTests
    {
        [Test]
        public void DeleteCommand_Parses_RequiredIdOption_Success()
        {
            // Arrange
            var requiredOptions = new string[] { "delete", "--id", "1" };

            // Act
            var result = Parser.Default.ParseArguments<DeleteCommand>(requiredOptions);

            // Assert
            Assert.That(result, Is.InstanceOf<Parsed<DeleteCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Value.Id, Is.EqualTo(1));
                Assert.That(result.Value.Force, Is.False);
            }
        }

        [Test]
        public void DeleteCommand_Parses_ForceOption_Success()
        {
            // Arrange
            var withForceOption = new string[] { "delete", "--id", "1", "--force", "true" };

            // Act
            var result = Parser.Default.ParseArguments<DeleteCommand>(withForceOption);

            // Assert
            Assert.That(result, Is.InstanceOf<Parsed<DeleteCommand>>());
            Assert.That(result.Value.Force, Is.True);
        }

        [Test]
        public void DeleteCommand_MissingId_Fails()
        {
            // Arrange
            var withIdMissing = new string[] { "delete" };

            // Act
            var result = Parser.Default.ParseArguments<DeleteCommand>(withIdMissing);

            // Assert
            Assert.That(result, Is.InstanceOf<NotParsed<DeleteCommand>>());
            Assert.That(result.Errors.OfType<MissingRequiredOptionError>()
                .Any(e => e.NameInfo.LongName == "id"), Is.True);
        }

        [Test]
        public void DeleteCommand_InvalidIdType_Fails()
        {
            // Arrange
            var withInvalidIdType = new string[] { "delete", "--id", "one" };

            // Act
            var result = Parser.Default.ParseArguments<DeleteCommand>(withInvalidIdType);

            // Assert
            Assert.That(result, Is.InstanceOf<NotParsed<DeleteCommand>>());
            Assert.That(result.Errors.OfType<BadFormatConversionError>()
                .Any(e => e.NameInfo.LongName == "id"), Is.True);
        }

        [Test]
        public void DeleteCommand_UnknownOption_Fails()
        {
            // Arrange
            var withUnknownOption = new string[] { "delete", "--ids", "1" };

            // Act
            var result = Parser.Default.ParseArguments<DeleteCommand>(withUnknownOption);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.NotParsed));
                Assert.That(result.Errors.OfType<UnknownOptionError>()
                    .Any(e => e.Token == "ids"), Is.True);
            }
        }

        [Test]
        public void DeleteCommand_EmptyArguments_Fails()
        {
            // Arrange
            var withEmptyArguments = new string[] { "delete", "--id", "" };

            // Act
            var result = Parser.Default.ParseArguments<DeleteCommand>(withEmptyArguments);

            // Arrange
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.NotParsed));
                Assert.That(result.Errors.OfType<BadFormatConversionError>()
                    .Any(e => e.NameInfo.LongName == "id"), Is.True);
            }
        }

        [Test]
        public void DeleteCommand_OptionalForce_DefaultValue_WhenNotProvided()
        {
            // Arrange
            var withForceDefault = new string[] { "delete", "--id", "12" };

            // Act
            var result = Parser.Default.ParseArguments<DeleteCommand>(withForceDefault);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Force, Is.False);
            }
        }
    }
}
