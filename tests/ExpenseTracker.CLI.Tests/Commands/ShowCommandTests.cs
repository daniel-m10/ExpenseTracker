using CommandLine;
using ExpenseTracker.CLI.Commands;

namespace ExpenseTracker.CLI.Tests.Commands
{
    [TestFixture]
    public class ShowCommandTests
    {
        [Test]
        public void ShowCommand_Parses_RequiredIdOption_Success()
        {
            // Arrange
            var withId = new string[] { "show", "--id", "7" };

            // Act
            var result = Parser.Default.ParseArguments<ShowCommand>(withId);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Id, Is.EqualTo(7));
            }
        }

        [Test]
        public void ShowCommand_MissingId_Fails()
        {
            // Arrange
            var withIdMissing = new string[] { "show" };

            // Act
            var result = Parser.Default.ParseArguments<ShowCommand>(withIdMissing);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.NotParsed));
                Assert.That(result.Errors.OfType<MissingRequiredOptionError>()
                    .Any(e => e.NameInfo.LongName == "id"), Is.True);
            }
        }

        [Test]
        public void ShowCommand_InvalidIdType_Fails()
        {
            // Arrange
            var withInvalidType = new string[] { "show", "--id", "ten" };

            // Act
            var result = Parser.Default.ParseArguments<ShowCommand>(withInvalidType);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.NotParsed));
                Assert.That(result.Errors.OfType<BadFormatConversionError>()
                    .Any(e => e.NameInfo.LongName == "id"), Is.True);
            }
        }

        [Test]
        public void ShowCommand_UnknownOption_Fails()
        {
            // Arrange
            var withUnknownOption = new string[] { "show", "--ids", "1" };

            // Act
            var result = Parser.Default.ParseArguments<ShowCommand>(withUnknownOption);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.NotParsed));
                Assert.That(result.Errors.OfType<UnknownOptionError>().
                    Any(e => e.Token == "ids"), Is.True);
            }
        }

        [Test]
        public void ShowCommand_EmptyArguments_Fails()
        {
            // Arrange
            var withEmptyArgument = new string[] { "show", "--id" };

            // Act
            var result = Parser.Default.ParseArguments<ShowCommand>(withEmptyArgument);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.NotParsed));
                Assert.That(result.Errors.OfType<MissingRequiredOptionError>().
                    Any(e => e.NameInfo.LongName == "id"), Is.True);
            }
        }
    }
}
