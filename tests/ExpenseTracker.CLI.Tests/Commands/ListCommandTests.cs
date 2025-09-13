using CommandLine;
using ExpenseTracker.CLI.Commands;

namespace ExpenseTracker.CLI.Tests.Commands
{
    [TestFixture]
    public class ListCommandTests
    {
        [Test]
        public void ListCommand_Parses_NoOptions_Success()
        {
            // Arrange
            var noOptions = new string[] { "list" };

            // Act
            var result = Parser.Default.ParseArguments<ListCommand>(noOptions);

            // Assert
            Assert.That(result, Is.InstanceOf<Parsed<ListCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Value.Month, Is.Null);
                Assert.That(result.Value.Year, Is.Null);
                Assert.That(result.Value.Category, Is.Null);
                Assert.That(result.Value.Limit, Is.Null);
            }
        }

        [Test]
        public void ListCommand_Parses_CategoryOption_Success()
        {
            // Arrange
            var withCategoryOption = new string[] { "list", "-c", "Food" };

            // Act
            var result = Parser.Default.ParseArguments<ListCommand>(withCategoryOption);

            // Assert
            Assert.That(result, Is.InstanceOf<Parsed<ListCommand>>());
            Assert.That(result.Value.Category, Is.EqualTo("Food"));
        }

        [Test]
        public void ListCommand_Parses_DateRangeOptions_Success()
        {
            // Arrange
            var withDateRangeOptions = new string[] { "list", "-m", "9", "-y", "2025" };

            // Act
            var result = Parser.Default.ParseArguments<ListCommand>(withDateRangeOptions);

            // Assert
            Assert.That(result, Is.InstanceOf<Parsed<ListCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Value.Month, Is.EqualTo(9));
                Assert.That(result.Value.Year, Is.EqualTo(2025));
            }
        }

        [TestCase("-m", "1", "-y", "2025", "-c", "Transport", "-l", "10")]
        [TestCase("--month", "5", "--year", "2025", "--category", "Food", "--limit", "2")]
        public void ListCommand_Parses_ShortAndLongOptions_Success(
            string month, string monthValue,
            string year, string yearValue,
            string category, string categoryValue,
            string limit, string limitValue)
        {
            // Arrange
            var shortLongOptions = new string[] { "list", month, monthValue, year, yearValue, category, categoryValue, limit, limitValue };

            // Act
            var result = Parser.Default.ParseArguments<ListCommand>(shortLongOptions);

            // Assert
            Assert.That(result, Is.InstanceOf<Parsed<ListCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Value.Month, Is.EqualTo(int.Parse(monthValue)));
                Assert.That(result.Value.Year, Is.EqualTo(int.Parse(yearValue)));
                Assert.That(result.Value.Limit, Is.EqualTo(int.Parse(limitValue)));
                Assert.That(result.Value.Category, Is.EqualTo(categoryValue));
            }
        }

        [Test]
        public void ListCommand_Parses_MixedOrderOfOptions_Success()
        {
            // Arrange
            var mixedOptions = new string[] { "list", "-c", "Transport", "--limit", "5" };
            // Act
            var result = Parser.Default.ParseArguments<ListCommand>(mixedOptions);
            // Assert
            Assert.That(result, Is.InstanceOf<Parsed<ListCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Value.Category, Is.EqualTo("Transport"));
                Assert.That(result.Value.Limit, Is.EqualTo(5));
            }
        }

        [Test]
        public void ListCommand_UnknownOption_Fails()
        {
            // Arrange
            var unknownOption = new string[] { "list", "-t", "2025" };

            // Act
            var result = Parser.Default.ParseArguments<ListCommand>(unknownOption);

            // Assert
            Assert.That(result, Is.InstanceOf<NotParsed<ListCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Errors, Is.Not.Empty);
                Assert.That(result.Errors.OfType<UnknownOptionError>()
                    .Any(e => e.Token == "t"), Is.True);
            }
        }

        [TestCase("-m", "")]
        [TestCase("-y", "")]
        public void ListCommand_InvalidDateFormat_Fails(string date, string value)
        {
            // Arrange
            var emptyArguments = new string[] { "list", date, value };

            // Act
            var result = Parser.Default.ParseArguments<ListCommand>(emptyArguments);

            // Assert
            Assert.That(result, Is.InstanceOf<NotParsed<ListCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Errors, Is.Not.Empty);
                Assert.That(
                    result.Errors.OfType<BadFormatConversionError>()
                    .Any(e => e.NameInfo.ShortName == date.Replace("-", string.Empty)),
                    Is.True);
            }
        }
    }
}
