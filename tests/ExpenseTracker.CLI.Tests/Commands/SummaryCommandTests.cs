using CommandLine;
using ExpenseTracker.CLI.Commands;

namespace ExpenseTracker.CLI.Tests.Commands
{
    [TestFixture]
    public class SummaryCommandTests
    {
        [Test]
        public void SummaryCommand_Parses_NoOptions_Success()
        {
            // Arrange
            var noOptions = new string[] { "summary" };

            // Act
            var result = Parser.Default.ParseArguments<SummaryCommand>(noOptions);

            // Assert
            Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
        }

        [Test]
        public void SummaryCommand_Parses_CategoryOption_Success()
        {
            // Arrange
            var withCategoryOption = new string[] { "summary", "-c", "Food" };

            // Act
            var result = Parser.Default.ParseArguments<SummaryCommand>(withCategoryOption);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Category, Is.EqualTo("Food"));
            }
        }

        [Test]
        public void SummaryCommand_Parses_DateRangeOptions_Success()
        {
            // Arrange
            var withDateRangeOptions = new string[] { "summary", "-m", "12", "-y", "2025" };

            // Act
            var result = Parser.Default.ParseArguments<SummaryCommand>(withDateRangeOptions);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Month, Is.EqualTo(12));
                Assert.That(result.Value.Year, Is.EqualTo(2025));
            }
        }

        [TestCase("-m", "1", "-y", "2025", "-c", "Transport")]
        [TestCase("--month", "10", "--year", "2025", "--category", "Clothes")]
        public void SummaryCommand_Parses_ShortAndLongOptions_Success(
            string month, string monthValue,
            string year, string yearValue,
            string category, string categoryValue
            )
        {
            // Arrange
            var withShortAndLongOptions = new string[] { "summary",
                month, monthValue,
                year, yearValue,
                category, categoryValue
            };

            // Act
            var result = Parser.Default.ParseArguments<SummaryCommand>(withShortAndLongOptions);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Month, Is.EqualTo(int.Parse(monthValue)));
                Assert.That(result.Value.Year, Is.EqualTo(int.Parse(yearValue)));
                Assert.That(result.Value.Category, Is.EqualTo(categoryValue));
            }
        }

        [Test]
        public void SummaryCommand_Parses_MixedOrderOfOptions_Success()
        {
            // Arrange
            var mixedOrderOptions = new string[] { "summary", "-c", "Food", "-y", "2025", "-m", "5" };

            // Act
            var result = Parser.Default.ParseArguments<SummaryCommand>(mixedOrderOptions);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Month, Is.EqualTo(5));
                Assert.That(result.Value.Year, Is.EqualTo(2025));
                Assert.That(result.Value.Category, Is.EqualTo("Food"));
            }
        }

        [Test]
        public void SummaryCommand_UnknownOption_Fails()
        {
            // Arrange
            var withUnknownOption = new string[] { "summary", "-d", "Food" };

            // Act
            var result = Parser.Default.ParseArguments<SummaryCommand>(withUnknownOption);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.NotParsed));
                Assert.That(result.Errors.OfType<UnknownOptionError>().
                    Any(e => e.Token == "d"), Is.True);
            }
        }

        [Test]
        public void SummaryCommand_EmptyArguments_ParsesDefaults()
        {
            // Arrange
            var withEmptyArguments = new string[] { "summary" };

            // Act
            var result = Parser.Default.ParseArguments<SummaryCommand>(withEmptyArguments);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Tag, Is.EqualTo(ParserResultType.Parsed));
                Assert.That(result.Value.Month, Is.Null);
                Assert.That(result.Value.Year, Is.Null);
                Assert.That(result.Value.Category, Is.Null);
            }
        }
    }
}
